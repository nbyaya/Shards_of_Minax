using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using Server.Spells.Fourth; // For FireFieldSpell

namespace Server.Mobiles
{
    [CorpseName("a smoldering orcish corpse")]
    public class InfernalOrc : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextFlameAuraTime;
        private DateTime m_NextFireNovaTime;
        private DateTime m_NextMoltenLeapTime;
        private Point3D m_LastLocation;

        // Unique fiery hue – 1161 is a bright, burning orange/red.
        private const int UniqueHue = 1161;

        [Constructable]
        public InfernalOrc() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Infernal Orc";
            Body = 138;             // Using the OrcishLord body
            BaseSoundID = 0x45A;      // Using the OrcishLord sound
            Hue = UniqueHue;

            // --- Enhanced Stats ---
            SetStr(300, 350);
            SetDex(130, 150);
            SetInt(150, 170);

            SetHits(500, 600);

            SetDamage(15, 25);

            // Split damage: a mix of physical and fire damage
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 60);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 70, 80);  // Very high fire resistance
            SetResistance(ResistanceType.Cold, 10, 20);    // Relatively vulnerable to cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Skills ---
            SetSkill(SkillName.MagicResist, 80.1, 95.0);
            SetSkill(SkillName.Swords, 80.1, 95.0);
            SetSkill(SkillName.Tactics, 85.1, 100.0);
            SetSkill(SkillName.Wrestling, 80.1, 95.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 55;
            ControlSlots = 4;

            // Initialize ability cooldowns.
            m_NextFlameAuraTime = DateTime.UtcNow;
            m_NextFireNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextMoltenLeapTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            m_LastLocation = this.Location;

            // --- Standard and Extra Loot ---
            PackItem(new RingmailChest());

            if (0.3 > Utility.RandomDouble())
                PackItem(Loot.RandomPossibleReagent());
            if (0.2 > Utility.RandomDouble())
                PackItem(new BolaBall());
            if (0.5 > Utility.RandomDouble())
                PackItem(new Yeast());
            PackItem(new FireRuby()); // Custom drop representing a fiery ingredient
        }

        // --- Flame Aura: Triggered when other mobiles move near the Infernal Orc ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            // Only trigger if the moving mobile is near and aura cooldown has expired
            if (m != null && m != this && m.Alive && this.Alive && m.Map == this.Map && m.InRange(this.Location, 2) && DateTime.UtcNow >= m_NextFlameAuraTime)
            {
                int itemID = 0x398C; // Graphic for a fire field
                TimeSpan duration = TimeSpan.FromSeconds(4 + Utility.Random(3));
                int damage = 3;

                // Spawn a temporary fire field at the moving mobile's old location
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Visual and sound effect
                m.FixedParticles(0x3709, 10, 20, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208);

                // Deal fire-only damage if the mobile can be harmed
                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0);
                }
                // Reset the aura cooldown (3 seconds)
                m_NextFlameAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(3);
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            // If the Infernal Orc has moved, leave behind a short-lived fire trail
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(6 + Utility.Random(3));
                int damage = 3;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            // Check for special abilities if there is a valid combatant.
            if (Combatant != null && Map != null && Map != Map.Internal)
            {
                if (Combatant is Mobile target)
                {
                    // Fire Nova: an area attack when the target is within 8 tiles.
                    if (DateTime.UtcNow >= m_NextFireNovaTime && this.InRange(target.Location, 8))
                    {
                        FireNovaAttack();
                        m_NextFireNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
                    }
                    // Molten Leap: if the target isn’t immediately adjacent, leap in for an AoE slam.
                    else if (DateTime.UtcNow >= m_NextMoltenLeapTime && !this.InRange(target.Location, 2))
                    {
                        MoltenLeapAttack(target);
                        m_NextMoltenLeapTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
                    }
                }
            }
        }

        // --- Fire Nova Attack: AoE explosion of fire damage ---
        public void FireNovaAttack()
        {
            if (Map == null)
                return;

            // Play initiating sound and effect
            PlaySound(0x208);
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
                // Create a visually striking explosion using the unique hue.
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 
                    0x3709, 10, 60, UniqueHue, 0, 5029, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(30, 45);
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                    target.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Molten Leap Attack: The Infernal Orc leaps toward its target and creates an explosive impact ---
        public void MoltenLeapAttack(Mobile target)
        {
            if (target == null || Map == null)
                return;

            // Optional flavor text and sound effect for the leap
            this.Say("*The Infernal Orc launches itself with molten fury!*");
            PlaySound(0x56C);

            // Determine a new location adjacent to the target (randomized within 1 tile)
            Point3D newLocation = new Point3D(
                target.X + Utility.RandomMinMax(-1, 1),
                target.Y + Utility.RandomMinMax(-1, 1),
                target.Z);

            // Ensure the new location is valid before teleporting
            if (Map.CanFit(newLocation.X, newLocation.Y, newLocation.Z, 16, false, false))
            {
                this.Location = newLocation;
                Effects.SendLocationParticles(EffectItem.Create(newLocation, this.Map, EffectItem.DefaultDuration),
                    0x3728, 10, 30, UniqueHue, 0, 5029, 0);
            }

            // On landing, affect all nearby mobiles within 2 tiles.
            IPooledEnumerable en = Map.GetMobilesInRange(this.Location, 2);
            foreach (Mobile m in en)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(20, 35);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.FixedParticles(0x3709, 10, 20, 5052, EffectLayer.Waist);
                }
            }
            en.Free();
        }

        // --- Death Explosion: On death, spawn hot lava tiles and a central explosion effect ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                int lavaTilesToDrop = 6;
                List<Point3D> effectLocations = new List<Point3D>();

                for (int i = 0; i < lavaTilesToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-2, 2);
                    int yOffset = Utility.RandomMinMax(-2, 2);
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

                    // Spawn HotLavaTile (assumed to be defined elsewhere)
                    HotLavaTile droppedLava = new HotLavaTile();
                    droppedLava.Hue = UniqueHue;
                    droppedLava.MoveToWorld(lavaLocation, this.Map);

                    // Create a quick flamestrike effect at this lava tile
                    Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration),
                        0x3709, 10, 20, UniqueHue, 0, 5016, 0);
                }

                // Choose one of the effect locations for a central explosion
                Point3D deathLocation = this.Location;
                if (effectLocations.Count > 0)
                    deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

                Effects.PlaySound(deathLocation, this.Map, 0x218);
                Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);
            }

            base.OnDeath(c);
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 3; } }
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus { get { return 50.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(3, 6));

            // A small chance for a special, themed drop.
            if (Utility.RandomDouble() < 0.02)
                PackItem(new MaxxiaScroll());
        }

        public InfernalOrc(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize ability cooldowns
            m_NextFlameAuraTime = DateTime.UtcNow;
            m_NextFireNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextMoltenLeapTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
        }
    }
}
