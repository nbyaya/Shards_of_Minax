using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a nightmare panther corpse")]
    public class NightmarePanther : BaseCreature
    {
        private DateTime m_NextNightmareVisions;
        private DateTime m_NextFearsomeRoar;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public NightmarePanther()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a nightmare panther";
            Body = 0xD6; // Panther body
            Hue = 2181; // Unique hue for the Nightmare Panther
            BaseSoundID = 0x462;

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

        public NightmarePanther(Serial serial)
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
                    Random rand = new Random();
                    m_NextNightmareVisions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextFearsomeRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextNightmareVisions)
                {
                    NightmareVisions();
                }

                if (DateTime.UtcNow >= m_NextFearsomeRoar)
                {
                    FearsomeRoar();
                }
            }
        }

        private void NightmareVisions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Nightmare Panther's eyes glow with dread! *");
            PlaySound(0x1F4); // Sound for the effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are overwhelmed by horrifying illusions!");
                    m.FixedEffect(0x373A, 10, 16); // Visual effect for the ability
                    m.Damage(Utility.RandomMinMax(5, 10), this); // Damage from the ability
                }
            }

            m_NextNightmareVisions = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30)); // Random cooldown
        }

        private void FearsomeRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Nightmare Panther roars fearfully! *");
            PlaySound(0x55F); // Sound for the roar effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are struck with panic and flee in terror!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Freezing effect
                }
            }

            m_NextFearsomeRoar = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 60)); // Random cooldown
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
            // Set initial random times for abilities
            Random rand = new Random();
            m_NextNightmareVisions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
            m_NextFearsomeRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
        }
    }
}
