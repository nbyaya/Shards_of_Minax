using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using System.Collections.Generic;
using Server.Spells.Fourth;

namespace Server.Mobiles
{
    [CorpseName("a charred llama corpse")]
    public class BlazingLlama : BaseCreature
    {
        // Timers for special abilities to prevent spamming
        private DateTime m_NextFireBurstTime;
        private DateTime m_NextFlameBlastTime;
        private Point3D m_LastLocation;
        // Unique fire hue—feel free to adjust to any desired fiery shade (e.g., 1161 is a bright orange/red)
        private const int UniqueHue = 1161;

        [Constructable]
        public BlazingLlama() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a Blazing Llama";
            Body = 0xDC;
            BaseSoundID = 0x3F3;
            Hue = UniqueHue;

            // --- Enhanced Stats ---
            SetStr(150, 200);
            SetDex(120, 150);
            SetInt(80, 120);

            SetHits(500, 600);
            SetStam(200, 250);
            SetMana(150, 200);

            SetDamage(15, 20);

            // Damage types: a mix of physical with a heavy emphasis on fire
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 85, 95);  // Extremely fire resistant!
            SetResistance(ResistanceType.Cold, 10, 20);   // Some vulnerability to cold
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Skills ---
            SetSkill(SkillName.EvalInt, 70.0, 85.0);
            SetSkill(SkillName.Magery, 70.0, 90.0);
            SetSkill(SkillName.MagicResist, 80.0, 95.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 40;
            ControlSlots = 3;
			Tamable = true;

            // Initialize ability cooldowns (FireBurst and FlameBlast)
            m_NextFireBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextFlameBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));

            // Sample loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(2, 5)));
            if (Utility.RandomDouble() < 0.02) // 2% chance for a special fiery drop
                PackItem(new MaxxiaScroll());

            m_LastLocation = this.Location;
        }

        // --- Ability: Fire Trail ---
        // Whenever the llama moves, it leaves behind a small burning patch.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (this.Alive && m != this && m.Map == this.Map && InRange(oldLocation, 2))
            {
                int itemID = 0x398C; // Using same item ID as FireField
                TimeSpan duration = TimeSpan.FromSeconds(4 + Utility.Random(3));
                int damage = 3;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Optional: Visual and sound effects on the target that moved near the llama's trail
                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208);
            }
            base.OnMovement(m, oldLocation);
        }

        // --- Ability: Flame Blast ---
        // Creates an AoE explosion around the Blazing Llama’s current location.
        public void FlameBlastAttack()
        {
            if (Map == null)
                return;

            // Visual and audio effects for explosion
            this.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);
            this.PlaySound(0x208);

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
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5029, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(20, 30);
                    // Fire-only damage distribution
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                    target.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Ability: Fire Burst ---
        // A targeted burst attack dealing moderate AoE damage to enemies near the current Combatant.
        public void FireBurstAttack()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            if (!InRange(target.Location, 8))
                return;

            this.Say("*Blazing charge!*");
            this.PlaySound(0x208);

            Effects.SendLocationParticles(EffectItem.Create(target.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5029, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(target.Location, 4);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(25, 40);
                AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                m.SendMessage("You are scorched by the sudden burst of flame!");
            }
        }

        // --- Overriding OnThink for ability use ---
        public override void OnThink()
        {
            base.OnThink();

            if (Map == null || Map == Map.Internal)
                return;

            // If the creature has moved, leave an additional fire trail at the previous location.
            if (this.Location != m_LastLocation)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(3));
                int damage = 3;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            // Check if there is a valid Combatant and use special abilities when off cooldown.
            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextFireBurstTime && InRange(Combatant.Location, 8))
                {
                    FireBurstAttack();
                    m_NextFireBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
                }
                else if (DateTime.UtcNow >= m_NextFlameBlastTime)
                {
                    FlameBlastAttack();
                    m_NextFlameBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
                }
            }
        }

        // --- Fiery Death Explosion ---
        // On death, the Blazing Llama scatters HotLavaTiles for additional dramatic effect.
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
                int xOffset = Utility.RandomMinMax(-2, 2);
                int yOffset = Utility.RandomMinMax(-2, 2);
                if (xOffset == 0 && yOffset == 0 && i < lavaTilesToDrop - 1)
                    xOffset = (Utility.RandomBool()) ? 1 : -1;

                Point3D lavaLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                {
                    lavaLocation.Z = Map.GetAverageZ(lavaLocation.X, lavaLocation.Y);
                    if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(lavaLocation);

                HotLavaTile droppedLava = new HotLavaTile();
                droppedLava.Hue = UniqueHue;
                droppedLava.MoveToWorld(lavaLocation, this.Map);
                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218);
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Overrides & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 4; } }
        public override double DispelDifficulty { get { return 80.0; } }
        public override double DispelFocus { get { return 40.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average, 1);
            if (Utility.RandomDouble() < 0.02) // 2% chance for a unique drop
            {
                PackItem(new MaxxiaScroll());
            }
        }

        // --- Serialization ---
        public BlazingLlama(Serial serial) : base(serial)
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

            m_NextFireBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextFlameBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
        }
    }
}
