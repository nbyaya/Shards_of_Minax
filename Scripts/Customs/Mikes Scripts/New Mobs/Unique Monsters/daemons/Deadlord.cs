using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Services;
using Server.Ethics;
using Server.Factions;

namespace Server.Mobiles
{
    [CorpseName("a Deadlord corpse")]
    public class Deadlord : BaseCreature
    {
        private DateTime m_NextMindControl;
        private DateTime m_NextDarkCommand;
        private DateTime m_NextFearsomeRoar;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Deadlord()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Deadlord";
            Body = 9; // Daemon body
            Hue = 1470; // Unique hue for the Deadlord
            BaseSoundID = 357;

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

        public Deadlord(Serial serial)
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

        public override double DispelDifficulty
        {
            get { return 150.0; }
        }

        public override double DispelFocus
        {
            get { return 55.0; }
        }

        public override Faction FactionAllegiance
        {
            get { return Shadowlords.Instance; }
        }

        public override Ethic EthicAllegiance
        {
            get { return Ethic.Evil; }
        }

        public override bool CanRummageCorpses
        {
            get { return true; }
        }

        public override Poison PoisonImmune
        {
            get { return Poison.Regular; }
        }

        public override int Meat
        {
            get { return 1; }
        }

        public override bool CanFly
        {
            get { return true; }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextMindControl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDarkCommand = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextFearsomeRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMindControl)
                {
                    MindControl();
                }

                if (DateTime.UtcNow >= m_NextDarkCommand)
                {
                    DarkCommand();
                }

                if (DateTime.UtcNow >= m_NextFearsomeRoar)
                {
                    FearsomeRoar();
                }
            }
        }

        private void MindControl()
        {
            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive && !target.Player)
            {
                target.SendMessage("You are overwhelmed by the Deadlord's will!");
                target.Freeze(TimeSpan.FromSeconds(10));
                target.Kill(); // Replace with actual mind control logic
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Deadlord controls its victim! *");

                m_NextMindControl = DateTime.UtcNow + TimeSpan.FromMinutes(2);
            }
        }

        private void DarkCommand()
        {
            Point3D loc = GetSpawnPosition(10);

            if (loc != Point3D.Zero)
            {
                this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Deadlord summons minions! *");

                for (int i = 0; i < 3; i++)
                {
                    Daemon minion = new Daemon();
                    minion.MoveToWorld(loc, Map);
                }

                m_NextDarkCommand = DateTime.UtcNow + TimeSpan.FromMinutes(3);
            }
        }

        private void FearsomeRoar()
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are struck with fear from the Deadlord's roar!");
                    m.Freeze(TimeSpan.FromSeconds(5));
                    m.Direction = (Direction)Utility.Random(8);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Deadlord roars fearfully! *");

            m_NextFearsomeRoar = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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
}
