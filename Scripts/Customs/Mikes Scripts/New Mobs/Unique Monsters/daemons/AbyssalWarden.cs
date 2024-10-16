using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an abyssal warden corpse")]
    public class AbyssalWarden : Daemon
    {
        private DateTime m_NextAbyssalChains;
        private DateTime m_NextDarkPortal;
        private DateTime m_NextSoulDrain;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AbyssalWarden()
            : base()
        {
            Name = "an abyssal warden";
            Body = 9; // Daemon body
            Hue = 1473; // Unique hue for Abyssal Warden
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

        public AbyssalWarden(Serial serial)
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
                    Random rand = new Random();
                    m_NextAbyssalChains = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDarkPortal = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextAbyssalChains)
                {
                    AbyssalChains();
                }

                if (DateTime.UtcNow >= m_NextDarkPortal)
                {
                    DarkPortal();
                }

                if (DateTime.UtcNow >= m_NextSoulDrain)
                {
                    SoulDrain();
                }
            }
        }

        private void AbyssalChains()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m != Combatant)
                {
                    if (m.Player)
                    {
                        m.SendMessage("Chains of the abyss ensnare you!");
                        m.Freeze(TimeSpan.FromSeconds(4));
                        m.Damage(20, this);
                    }
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Abyssal Chains lash out and bind the enemies! *");
            m_NextAbyssalChains = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown
        }

        private void DarkPortal()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant)
                {
                    if (m.Player)
                    {
                        Point3D loc = GetHazardousLocation();
                        m.MoveToWorld(loc, Map);
                        m.SendMessage("A dark portal transports you to a hazardous place!");
                    }
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A dark portal opens and enemies are sucked into it! *");
            m_NextDarkPortal = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Set cooldown
        }

        private void SoulDrain()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player && m.Alive)
                {
                    int drainAmount = 15;
                    m.Damage(drainAmount, this);
                    this.Hits += drainAmount;
                    this.SendMessage("The Abyssal Warden drains your soul!");

                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Soul Drain absorbs the life force from nearby enemies! *");
                }
            }

            m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Set cooldown
        }

        private Point3D GetHazardousLocation()
        {
            // Define a hazardous location for teleportation; this is just an example
            int x = X + Utility.RandomMinMax(-10, 10);
            int y = Y + Utility.RandomMinMax(-10, 10);
            int z = Map.GetAverageZ(x, y);

            Point3D p = new Point3D(x, y, z);
            return p;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
