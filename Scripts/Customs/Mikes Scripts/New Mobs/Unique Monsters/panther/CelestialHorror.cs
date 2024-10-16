using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a celestial horror corpse")]
    public class CelestialHorror : BaseCreature
    {
        private DateTime m_NextStarfall;
        private DateTime m_NextCosmicHowl;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CelestialHorror()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a celestial horror";
            Body = 0xD6; // Panther body
            Hue = 2185; // Celestial hue (light blue with star patterns)
			BaseSoundID = 0x462; // Panther sound

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
        }

        public CelestialHorror(Serial serial)
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
                    m_NextStarfall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCosmicHowl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextStarfall)
                {
                    Starfall();
                }

                if (DateTime.UtcNow >= m_NextCosmicHowl)
                {
                    CosmicHowl();
                }
            }
        }

        private void Starfall()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summoning starfall! *");
            PlaySound(0x20F);
            FixedEffect(0x3709, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("Falling stars explode around you!");
                }
            }

            m_NextStarfall = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Starfall
        }

        private void CosmicHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Howling with cosmic power! *");
            PlaySound(0x20F);
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are disoriented by the cosmic howl!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.SendMessage("You feel your movements slow down!");
                }
            }

            m_NextCosmicHowl = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for CosmicHowl
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

            // Reset initialization flag on deserialization
            m_AbilitiesInitialized = false;
        }
    }
}
