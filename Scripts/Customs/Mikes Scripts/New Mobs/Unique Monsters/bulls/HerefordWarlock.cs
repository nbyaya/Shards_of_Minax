using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a hereford warlock corpse")]
    public class HerefordWarlock : BaseCreature
    {
        private static readonly string[] m_Messages = new string[]
        {
            "*casts a shadow bolt*",
            "*surrounds the area with dark aura*",
            "*summons shadowy minions*"
        };

        private DateTime m_NextShadowBolt;
        private DateTime m_NextDarkAura;
        private DateTime m_NextSummonMinions;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public HerefordWarlock()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Hereford Warlock";
            Body = Utility.RandomList(0xE8, 0xE9); // Bull body
            Hue = 1285; // Unique hue
			BaseSoundID = 0x64;

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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public HerefordWarlock(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextShadowBolt = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDarkAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowBolt)
                {
                    ShadowBolt();
                }

                if (DateTime.UtcNow >= m_NextDarkAura)
                {
                    DarkAura();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonMinions();
                }
            }
        }

        private void ShadowBolt()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(20, 30);
                target.Damage(damage, this);
                target.SendMessage("A bolt of dark energy strikes you!");

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, m_Messages[0]);
                FixedEffect(0x3728, 10, 16);
                m_NextShadowBolt = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void DarkAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, m_Messages[1]);
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You feel weakened by the dark aura!");
                    m.Damage(10, this); // Weakening effect
                }
            }

            m_NextDarkAura = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, m_Messages[2]);
            FixedEffect(0x373A, 10, 16);

            Point3D loc = GetSpawnPosition(2);
            if (loc != Point3D.Zero)
            {
                ShadowMinion minion = new ShadowMinion();
                minion.MoveToWorld(loc, Map);

                Timer.DelayCall(TimeSpan.FromMinutes(1), new TimerCallback(delegate() 
                {
                    if (!minion.Deleted)
                        minion.Delete();
                }));

                m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset ability intervals when deserialized
            m_AbilitiesInitialized = false;
        }
    }

    public class ShadowMinion : BaseCreature
    {
        public ShadowMinion()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow minion";
            Body = Utility.RandomList(0xE8, 0xE9); // Bull body, or you can choose a different body
            Hue = 1155; // Darker hue

            SetStr(50);
            SetDex(50);
            SetInt(50);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 60.0, 70.0);
            SetSkill(SkillName.Magery, 60.0, 70.0);
            SetSkill(SkillName.Meditation, 60.0, 70.0);
            SetSkill(SkillName.MagicResist, 60.0, 70.0);
            SetSkill(SkillName.Tactics, 60.0);
            SetSkill(SkillName.Wrestling, 60.0);

            VirtualArmor = 30;
        }

        public ShadowMinion(Serial serial)
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
