using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an elder tendril corpse")]
    public class ElderTendril : BaseCreature
    {
        private DateTime m_NextVineControl;
        private DateTime m_NextRootedFury;
        private bool m_IsRooted;
        private List<Mobile> m_Minions;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ElderTendril()
            : this("Elder Tendril")
        {
        }

        [Constructable]
        public ElderTendril(string name)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 684;
            Hue = 1387; // Dark green hue
            this.Body = 8;
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

            m_IsRooted = false;
            m_Minions = new List<Mobile>();
            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public ElderTendril(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 7; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish | FoodType.FruitsAndVegies; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextVineControl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 30));
                    m_NextRootedFury = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVineControl)
                {
                    VineControl();
                }

                if (DateTime.UtcNow >= m_NextRootedFury && !m_IsRooted)
                {
                    RootedFury();
                }
            }
        }

        public void VineControl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Elder Tendril commands its minions!*");
            PlaySound(0x2F);

            for (int i = 0; i < m_Minions.Count; i++)
            {
                if (m_Minions[i].Deleted || !m_Minions[i].Alive)
                {
                    m_Minions.RemoveAt(i);
                    i--;
                }
            }

            if (m_Minions.Count < 3)
            {
                Point3D loc = GetSpawnPosition(2);

                if (loc != Point3D.Zero)
                {
                    LesserVine vine = new LesserVine(this);
                    vine.MoveToWorld(loc, Map);
                    m_Minions.Add(vine);
                }
            }

            m_NextVineControl = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset to a fixed interval for next use
        }

        public void RootedFury()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Elder Tendril roots itself for increased power!*");
            PlaySound(0x214);

            m_IsRooted = true;
            Frozen = true;

            DamageMax += 10;
            DamageMin += 5;

            foreach (ResistanceType res in Enum.GetValues(typeof(ResistanceType)))
            {
                SetResistance(res, GetResistance(res) + 15);
            }

            FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);

            Timer.DelayCall(TimeSpan.FromSeconds(15), () =>
            {
                m_IsRooted = false;
                Frozen = false;
                DamageMax -= 10;
                DamageMin -= 5;

                foreach (ResistanceType res in Enum.GetValues(typeof(ResistanceType)))
                {
                    SetResistance(res, GetResistance(res) - 15);
                }

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Elder Tendril uproots itself*");
            });

            m_NextRootedFury = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Reset to a fixed interval for next use
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

        public override int GetIdleSound()
        {
            return 443;
        }

        public override int GetAngerSound()
        {
            return 442;
        }

        public override int GetHurtSound()
        {
            return 445;
        }

        public override int GetDeathSound()
        {
            return 447;
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            for (int i = 0; i < m_Minions.Count; i++)
            {
                if (!m_Minions[i].Deleted && m_Minions[i].Alive)
                {
                    m_Minions[i].Kill();
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_Minions.Count);
            for (int i = 0; i < m_Minions.Count; i++)
            {
                writer.Write(m_Minions[i]);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            int count = reader.ReadInt();
            m_Minions = new List<Mobile>();
            for (int i = 0; i < count; i++)
            {
                LesserVine vine = reader.ReadMobile() as LesserVine;
                if (vine != null && !vine.Deleted)
                {
                    m_Minions.Add(vine);
                }
            }

            // Reset abilities initialization and intervals on deserialization
            m_AbilitiesInitialized = false;
            m_NextVineControl = DateTime.UtcNow;
            m_NextRootedFury = DateTime.UtcNow;
            m_IsRooted = false;
        }
    }

    public class LesserVine : BaseCreature
    {
        private Mobile m_Master;

        [Constructable]
        public LesserVine(Mobile master)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Name = "a lesser vine";
            Body = 8;
            Hue = 1272;
            BaseSoundID = 352;

            SetStr(96, 120);
            SetDex(66, 85);
            SetInt(16, 30);

            SetHits(58, 72);

            SetDamage(6, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 20.0, 30.0);
            SetSkill(SkillName.Tactics, 20.0, 30.0);
            SetSkill(SkillName.Wrestling, 20.0, 30.0);

            Fame = 0;
            Karma = 0;
            VirtualArmor = 16;
        }

        public LesserVine(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (m_Master == null || m_Master.Deleted || !m_Master.Alive)
            {
                Delete();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Master = reader.ReadMobile();
        }
    }
}
