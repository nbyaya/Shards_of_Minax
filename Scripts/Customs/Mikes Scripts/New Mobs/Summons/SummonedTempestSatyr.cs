using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a tempest satyr's corpse")]
    public class SummonedTempestSatyr : BaseCreature
    {
        private DateTime m_NextStormSong;
        private DateTime m_NextThunderousRhapsody;
        private DateTime m_NextGaleOfFury;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SummonedTempestSatyr()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a tempest satyr";
            Body = 271; // Using Satyr body
            Hue = 2297; // Unique hue
			this.BaseSoundID = 0x586;

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
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SummonedTempestSatyr(Serial serial)
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
                    m_NextStormSong = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextThunderousRhapsody = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextGaleOfFury = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextStormSong)
                {
                    StormSong();
                }

                if (DateTime.UtcNow >= m_NextThunderousRhapsody)
                {
                    ThunderousRhapsody();
                }

                if (DateTime.UtcNow >= m_NextGaleOfFury)
                {
                    GaleOfFury();
                }
            }
        }

        private void StormSong()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tempest Satyr sings a storm song! *");
            PlaySound(0x64E); // Thunder sound
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 40);
                    m.Damage(damage, this);
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextStormSong = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ThunderousRhapsody()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tempest Satyr plays a thunderous rhapsody! *");
            PlaySound(0x65C); // Thunderclap sound
            FixedEffect(0x37C4, 10, 36);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are knocked back by the shockwave!");
                    m.MoveToWorld(new Point3D(m.X + Utility.RandomMinMax(-2, 2), m.Y + Utility.RandomMinMax(-2, 2), m.Z), m.Map);
                }
            }

            m_NextThunderousRhapsody = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void GaleOfFury()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tempest Satyr summons a gale of fury! *");
            PlaySound(0x66B); // Wind sound
            FixedEffect(0x3779, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The gale disrupts your movements!");
                    m.Dex -= 10;
                    m.FixedEffect(0x376A, 10, 16);
                }
            }

            m_NextGaleOfFury = DateTime.UtcNow + TimeSpan.FromSeconds(60);
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
