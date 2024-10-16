using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a jersey enchantress corpse")]
    public class JerseyEnchantress : BaseCreature
    {
        private DateTime m_NextCharm;
        private DateTime m_NextMysticShield;
        private DateTime m_NextEnchantedMilk;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        private static readonly TimeSpan CharmDelay = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan MysticShieldDelay = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan EnchantedMilkDelay = TimeSpan.FromMinutes(1);

        [Constructable]
        public JerseyEnchantress()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a jersey enchantress";
            Body = Utility.RandomList(0xE8, 0xE9); // Using Bull body
            Hue = 1278; // Unique hue
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

        public JerseyEnchantress(Serial serial)
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
                    m_NextCharm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMysticShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEnchantedMilk = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextCharm)
                {
                    CastCharm();
                }

                if (DateTime.UtcNow >= m_NextMysticShield)
                {
                    CastMysticShield();
                }

                if (DateTime.UtcNow >= m_NextEnchantedMilk)
                {
                    CastEnchantedMilk();
                }
            }
        }

        private void CastCharm()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Casts a charm spell! *");
                Say("* You are under my control! *");

                // Find enemies within range and apply charm effect
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m != Combatant && m.Player)
                    {
                        // Apply charm logic here
                    }
                }

                m_NextCharm = DateTime.UtcNow + CharmDelay;
            }
        }

        private void CastMysticShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Creates a mystic shield! *");
            PlaySound(0x20C);
            FixedEffect(0x376A, 10, 16);

            // Increase own resistances temporarily
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(() => RemoveMysticShield()));
            m_NextMysticShield = DateTime.UtcNow + MysticShieldDelay;
        }

        private void RemoveMysticShield()
        {
            // Reset resistances
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);
        }

        private void CastEnchantedMilk()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Dispenses enchanted milk! *");
            PlaySound(0x20C);
            FixedEffect(0x376A, 10, 16);

            // Heal and remove negative effects from allies
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature && m != this)
                {
                    BaseCreature creature = (BaseCreature)m;
                    creature.Heal(20);
                    creature.SendMessage("You feel rejuvenated by the enchanted milk!");
                }
            }

            m_NextEnchantedMilk = DateTime.UtcNow + EnchantedMilkDelay;
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
