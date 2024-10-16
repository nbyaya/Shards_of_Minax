using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a coral sentinel corpse")]
    public class CoralSentinel : BaseCreature
    {
        private DateTime m_NextCoralSpikes;
        private DateTime m_NextReefBarrier;
        private DateTime m_NextTideSurge;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public CoralSentinel()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a coral sentinel";
            Body = 16; // Water Elemental body
            Hue = 2532; // Coral-like hue
            BaseSoundID = 278;

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
            CanSwim = true;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public CoralSentinel(Serial serial)
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

        public override double DispelDifficulty
        {
            get { return 120.0; }
        }

        public override double DispelFocus
        {
            get { return 50.0; }
        }

        public override bool BleedImmune
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
                    m_NextCoralSpikes = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextReefBarrier = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 10));
                    m_NextTideSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextCoralSpikes)
                {
                    CoralSpikes();
                }

                if (DateTime.UtcNow >= m_NextReefBarrier)
                {
                    ReefBarrier();
                }

                if (DateTime.UtcNow >= m_NextTideSurge)
                {
                    TideSurge();
                }
            }
        }

        private void CoralSpikes()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("Sharp coral spikes erupt from the ground, cutting into you!");
                    m.Damage(15, this);
                    m.AddToBackpack(new BleedingWound()); // Assuming BleedingWound is a custom item or effect
                }
            }

            m_NextCoralSpikes = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for CoralSpikes
        }

        private void ReefBarrier()
        {
            this.VirtualArmor += 20;
            this.SendMessage("A protective barrier of coral surrounds the Coral Sentinel!");

            m_NextReefBarrier = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for ReefBarrier
        }

        private void TideSurge()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("A surge of water crashes over you, pushing you away!");
                    m.Damage(5, this);
                    m.Location = new Point3D(m.X + Utility.RandomMinMax(-2, 2), m.Y + Utility.RandomMinMax(-2, 2), m.Z);
                }
            }

            m_NextTideSurge = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for TideSurge
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);

            writer.Write(m_NextCoralSpikes);
            writer.Write(m_NextReefBarrier);
            writer.Write(m_NextTideSurge);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_NextCoralSpikes = reader.ReadDateTime();
                    m_NextReefBarrier = reader.ReadDateTime();
                    m_NextTideSurge = reader.ReadDateTime();
                    break;
            }

            // Reset the initialization flag on deserialization
            m_AbilitiesInitialized = false;
        }
    }

    // Assuming BleedingWound is a custom item or effect that causes bleeding
    public class BleedingWound : Item
    {
        [Constructable]
        public BleedingWound()
            : base(0x176B) // Example item ID
        {
            Movable = false;
            Hue = 1150; // Example hue
        }

        public BleedingWound(Serial serial)
            : base(serial)
        {
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
        }
    }
}
