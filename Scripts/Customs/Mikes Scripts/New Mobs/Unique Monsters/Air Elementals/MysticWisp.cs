using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mystic wisp corpse")]
    public class MysticWisp : BaseCreature
    {
        private static readonly string[] m_Vocabulary = new string[]
        {
            "Feel the magic!",
            "I am the wisp of dreams!",
            "Can you see me now?"
        };

        private DateTime m_NextConfusion;
        private DateTime m_NextIllusion;
        private bool m_AbilitiesActivated; // New flag to track initial ability activation

        [Constructable]
        public MysticWisp()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a mystic wisp";
            Body = 13; // Ghost-like body
            Hue = 1080; // Blue hue
			BaseSoundID = 655;

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
            SetResistance(ResistanceType.Poison, 100);
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

            m_AbilitiesActivated = false; // Initialize flag
        }

        public MysticWisp(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override int Meat { get { return 0; } }
        public override FoodType FavoriteFood { get { return FoodType.None; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesActivated)
                {
                    // Randomly set the initial activation times
                    Random rand = new Random();
                    m_NextConfusion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextIllusion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextConfusion)
                {
                    CastConfusion();
                }

                if (DateTime.UtcNow >= m_NextIllusion)
                {
                    CreateIllusion();
                }
            }
        }

        private void CastConfusion()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You feel disoriented as the Mystic Wisp's magic affects you!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextConfusion = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Adjusted to ensure consistency in intervals
        }

        private void CreateIllusion()
        {
            Point3D loc = GetSpawnPosition(2);

            if (loc != Point3D.Zero)
            {
                Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);

                Illusion clone = new Illusion();
                clone.MoveToWorld(loc, Map);

                Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate() 
                {
                    if (!clone.Deleted)
                        clone.Delete(); 
                }));

                m_NextIllusion = DateTime.UtcNow + TimeSpan.FromMinutes(3); // Adjusted to ensure consistency in intervals
            }
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
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesActivated = false; // Reset flag
        }
    }

    public class Illusion : BaseCreature
    {
        public Illusion()
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Body = 0x1D; // Ghost-like body
            Hue = 1155; // Blue hue

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);
            SetDamage(0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;
        }

        public Illusion(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
