using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;          // For additional spell effects
using Server.Network;         // For visual/sound effects
using System.Collections.Generic;
using Server.Spells.Fourth;   // For firefield effects

namespace Server.Mobiles
{
    [CorpseName("a scorched ghostly corpse")]
    public class BlazingShade : BaseCreature
    {
        // Timers for special abilities to help prevent spamming
        private DateTime m_NextAuraTime;
        private DateTime m_NextNovaTime;
        private DateTime m_NextChainsTime;
        private Point3D m_LastLocation;

        // Unique hue for the fire theme
        private const int UniqueHue = 1175; 

        [Constructable]
        public BlazingShade() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a Blazing Shade";
            Body = 26;               // Same body as the original Shade
            BaseSoundID = 0x482;       // Same base sound ID as Shade
            Hue = UniqueHue;

            // --- Enhanced Stats ---
            SetStr(300, 350);
            SetDex(200, 250);
            SetInt(200, 250);

            SetHits(900, 1000);       // Much higher health pool
            SetStam(200, 250);        // Increased stamina
            SetMana(200, 250);        // Increased mana

            SetDamage(25, 30);        // Higher base damage
            // Damage split: part physical, primarily fire
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            // --- Adjusted Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, -10, 5); // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 90.1, 110.0);
            SetSkill(SkillName.Magery, 90.1, 110.0);
            SetSkill(SkillName.MagicResist, 100.1, 120.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 80.1, 100.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 50;
            ControlSlots = 4;
			Tamable = true;

            // --- Initialize Ability Timers ---
            m_NextAuraTime = DateTime.UtcNow;
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            m_NextChainsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));

            m_LastLocation = this.Location;

            // Standard (and extra) loot ingredients
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 6)));

            // Optional: add a light source so the creature glows
            AddItem(new LightSource());
        }

        public override bool BleedImmune { get { return true; } }
        public override TribeType Tribe { get { return TribeType.Undead; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        // --- Flame Aura: Leaves behind burning fields when others move near it ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive)
            {
                int itemID = 0x398C; // Fire field graphic ID
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                int damage = 3; // Slightly more potent than the baseline aura

                // Create a temporary fire field at the mobile's old location
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Visual and audio feedback for stepping into the flames
                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208);

                // Damage the mobile with fire damage if valid
                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- Main Thinking Logic for Special Abilities ---
        public override void OnThink()
        {
            base.OnThink();

            // Drop a flame field if the creature has moved since the last think cycle
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(5));
                int damage = 3;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Ability: Flame Nova (AoE burst)
            if (DateTime.UtcNow >= m_NextNovaTime && this.InRange(Combatant.Location, 6))
            {
                FlameNovaAttack();
                m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Ability: Searing Chains (targeted drain and damage)
            else if (DateTime.UtcNow >= m_NextChainsTime && this.InRange(Combatant.Location, 8))
            {
                SearingChainsAttack();
                m_NextChainsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
        }

        // --- Unique Ability: Flame Nova ---
        // Releases a burst of fire damaging all nearby targets and leaves a short burning effect.
        public void FlameNovaAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x208); // Fireball-like sound
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                // Expanding visual effect centered on BlazingShade
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5029, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(35, 50);
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                    
                    // Apply a brief burning over time effect (using a delayed tick)
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        if (target != null && target.Alive && CanBeHarmful(target, false))
                            AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0);
                    });
                    target.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Unique Ability: Searing Chains ---
        // Targets the current Combatant, dealing fire damage while draining mana and stamina.
        public void SearingChainsAttack()
        {
            if (Combatant == null || Map == null)
                return;

            // Make sure the Combatant is a Mobile before accessing Mobile-specific properties
            if (Combatant is Mobile target)
            {
                if (!CanBeHarmful(target))
                    return;

                this.Say("*Burning chains ensnare you!*");
                PlaySound(0x5A); // Sound for chains hitting

                DoHarmful(target);
                int damage = Utility.RandomMinMax(20, 30);
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

                // Drain some of the target's mana and stamina
                target.Mana = Math.Max(0, target.Mana - 15);
                target.Stam = Math.Max(0, target.Stam - 15);

                // Visual chained-flames effect on the target
                target.FixedParticles(0x376A, 10, 20, 5032, UniqueHue, 0, EffectLayer.Waist);
            }
        }

        // --- Death Explosion ---
        // On death, BlazingShade spawns several burning lava tiles and an explosive effect.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int lavaTilesToDrop = 8;
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < lavaTilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);
                if (xOffset == 0 && yOffset == 0 && i < lavaTilesToDrop - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D lavaLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                {
                    lavaLocation.Z = Map.GetAverageZ(lavaLocation.X, lavaLocation.Y);
                    if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(lavaLocation);

                // Spawn the burning tile (assumes HotLavaTile is defined elsewhere)
                HotLavaTile droppedLava = new HotLavaTile();
                droppedLava.Hue = UniqueHue;
                droppedLava.MoveToWorld(lavaLocation, this.Map);

                // Create a quick flamestrike effect at the lava tile
                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            // Play a central explosion effect near one of the spawned lava tiles
            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218);
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));



            // Additional rare resource drop example
            if (Utility.RandomDouble() < 0.05)
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
        }

        public BlazingShade(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextAuraTime);
            writer.Write(m_NextNovaTime);
            writer.Write(m_NextChainsTime);
            writer.Write(m_LastLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextAuraTime = reader.ReadDateTime();
            m_NextNovaTime = reader.ReadDateTime();
            m_NextChainsTime = reader.ReadDateTime();
            m_LastLocation = reader.ReadPoint3D();

            // Reinitialize timers if necessary
            if (m_NextAuraTime < DateTime.UtcNow)
                m_NextAuraTime = DateTime.UtcNow;
            if (m_NextNovaTime < DateTime.UtcNow)
                m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            if (m_NextChainsTime < DateTime.UtcNow)
                m_NextChainsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
        }
    }
}
