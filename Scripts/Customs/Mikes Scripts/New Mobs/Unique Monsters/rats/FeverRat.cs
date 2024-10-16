using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a fever rat corpse")]
    public class FeverRat : GiantRat
    {
        private static readonly int FeverHue = 2267; // Unique hue for Fever Rat
        protected internal static readonly int FeverMinionBody = 0xD7; // Body ID for the minions

        private DateTime m_NextBurningBite;
        private DateTime m_NextFeverAura;
        private DateTime m_NextSummonMinions;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FeverRat()
            : base()
        {
            Name = "a fever rat";
            Hue = FeverHue; // Apply unique hue
            this.BaseSoundID = 0xCC;
            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public FeverRat(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextBurningBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextFeverAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 3));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBurningBite)
                {
                    PerformBurningBite();
                }

                if (DateTime.UtcNow >= m_NextFeverAura)
                {
                    ApplyFeverAura();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonFeverMinions();
                }
            }
        }

        private void PerformBurningBite()
        {
            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive)
            {
                // Display message
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* You feel an intense fever spreading through your body! *");

                // Effect: Red screen flash
                target.SendMessage("You feel an intense fever spreading through your body!");
                target.FixedEffect(0x374A, 10, 16); // Red screen effect

                // Apply fever effect
                int staminaReduction = Utility.RandomMinMax(10, 15);
                target.Stam = Math.Max(0, target.Stam - staminaReduction);

                // Apply fire damage over time
                int fireDamage = Utility.RandomMinMax(10, 20);
                target.Damage(fireDamage, this);
                target.SendMessage("The fever makes you more vulnerable to fire!");

                // Apply fever to nearby enemies
                ApplyFeverToNearbyEnemies();

                m_NextBurningBite = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void ApplyFeverToNearbyEnemies()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m is PlayerMobile)
                {
                    m.SendMessage("You feel an intense fever spreading from the Fever Rat!");
                    m.FixedEffect(0x374A, 10, 16); // Red screen effect

                    int staminaReduction = Utility.RandomMinMax(5, 10);
                    m.Stam = Math.Max(0, m.Stam - staminaReduction);

                    int fireDamage = Utility.RandomMinMax(5, 10);
                    m.Damage(fireDamage, this);
                }
            }
        }

        private void ApplyFeverAura()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && m is PlayerMobile)
                {
                    m.SendMessage("The feverish aura around the Fever Rat weakens you!");
                    m.FixedEffect(0x374A, 10, 16); // Red screen effect

                    int staminaReduction = Utility.RandomMinMax(1, 5);
                    m.Stam = Math.Max(0, m.Stam - staminaReduction);

                    m.SendMessage("You are more vulnerable to fire damage!");
                    m.Damage(Utility.RandomMinMax(1, 5), this);
                }
            }

            m_NextFeverAura = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void SummonFeverMinions()
        {
            for (int i = 0; i < 2; i++)
            {
                FeverMinion minion = new FeverMinion();
                Point3D loc = GetSpawnPosition(3);

                if (loc != Point3D.Zero)
                {
                    minion.MoveToWorld(loc, Map);
                    minion.Combatant = Combatant;
                }
            }

            m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class FeverMinion : BaseCreature
    {
        [Constructable]
        public FeverMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a fever minion";
            Body = FeverRat.FeverMinionBody;
            Hue = 1154; // Match Fever Rat hue

            this.SetStr(30, 50);
            this.SetDex(30, 50);
            this.SetInt(10, 20);
            this.SetHits(20, 30);
            this.SetDamage(4, 8);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 10, 20);
            this.SetResistance(ResistanceType.Fire, 15, 25);
            this.SetResistance(ResistanceType.Poison, 30, 40);

            this.SetSkill(SkillName.MagicResist, 15.0, 25.0);
            this.SetSkill(SkillName.Tactics, 15.0, 25.0);
            this.SetSkill(SkillName.Wrestling, 15.0, 25.0);
        }

        public FeverMinion(Serial serial)
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
        }
    }
}
