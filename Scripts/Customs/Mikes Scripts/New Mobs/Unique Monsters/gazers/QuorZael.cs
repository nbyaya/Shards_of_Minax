using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a quor'zael corpse")]
    public class QuorZael : ElderGazer
    {
        private DateTime m_NextTemporalDistortion;
        private DateTime m_NextForesight;
        private DateTime m_NextTimeLeap;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public QuorZael() : base()
        {
            Name = "Quor'Zael the Harbinger";
            Hue = 1768; // Unique hue for Quor'Zael
            Body = 22; // ElderGazer body
			BaseSoundID = 377;

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

        public QuorZael(Serial serial) : base(serial)
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
                    m_NextTemporalDistortion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextForesight = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextTimeLeap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTemporalDistortion)
                {
                    UseTemporalDistortion();
                }

                if (DateTime.UtcNow >= m_NextForesight)
                {
                    UseForesight();
                }

                if (DateTime.UtcNow >= m_NextTimeLeap)
                {
                    UseTimeLeap();
                }
            }
        }

        private void UseTemporalDistortion()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Temporal Distortion! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("Time seems to slow down around you!");
                    // Here you would add code to slow down the target's attack speed and movement
                    // This is a placeholder; implementation will vary based on your server setup
                }
            }

            m_NextTemporalDistortion = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Temporal Distortion
        }

        private void UseForesight()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Foresight! *");
            FixedEffect(0x376A, 10, 16);

            // Here you would add code to increase Quor'Zael's chance to dodge attacks
            // This is a placeholder; implementation will vary based on your server setup

            m_NextForesight = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Foresight
        }

        private void UseTimeLeap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Time Leap! *");
            FixedEffect(0x376A, 10, 16);

            Point3D newLocation = GetRandomLocation();
            MoveToWorld(newLocation, Map);

            m_NextTimeLeap = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for Time Leap
        }

        private Point3D GetRandomLocation()
        {
            // Finds a random location near the current location
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-10, 10);
                int y = Y + Utility.RandomMinMax(-10, 10);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return new Point3D(X, Y, Z); // Default to current location if no valid location found
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
