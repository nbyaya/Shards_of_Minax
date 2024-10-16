using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an ancient alligator corpse")]
    public class AncientAlligator : BaseCreature
    {
        private DateTime m_NextEarthquake;
        private DateTime m_NextStoneSkin;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AncientAlligator()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ancient alligator";
            Body = 0xCA; // Alligator body
            Hue = 1173; // Unique hue
            BaseSoundID = 660;

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

        public AncientAlligator(Serial serial)
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
                    m_NextEarthquake = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60)); // Random start between 10 and 60 seconds
                    m_NextStoneSkin = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90)); // Random start between 15 and 90 seconds
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEarthquake)
                {
                    CastEarthquake();
                }

                if (DateTime.UtcNow >= m_NextStoneSkin)
                {
                    CastStoneSkin();
                }
            }
        }

        private void CastEarthquake()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The ground shakes violently as the Ancient Alligator uses earthquake!");
                    m.Damage(50, this);
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ancient Alligator causes a massive earthquake! *");
            m_NextEarthquake = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for Earthquake
        }

        private void CastStoneSkin()
        {
            this.SendMessage("The Ancient Alligator's skin hardens like stone!");
            this.VirtualArmor = 70;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.VirtualArmor = 50;
            });

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ancient Alligator's skin becomes incredibly tough! *");
            m_NextStoneSkin = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for StoneSkin
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
