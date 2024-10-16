using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a shadow crab corpse")]
    public class ShadowCrab : BaseMount
    {
        private DateTime m_NextShadowPull;
        private DateTime m_NextDarkSlash;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ShadowCrab()
            : base("Shadow Crab", 1510, 16081, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Body = 1510; // Coconut Crab body
            Hue = 1398; // Dark hue for shadow effect
            BaseSoundID = 0x4F2;

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

        public ShadowCrab(Serial serial)
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
                    m_NextShadowPull = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDarkSlash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowPull)
                {
                    ShadowPull();
                }

                if (DateTime.UtcNow >= m_NextDarkSlash)
                {
                    DarkSlash();
                }
            }
        }

        private void ShadowPull()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Crab pulls its enemy into the shadows! *");
            PlaySound(0x657);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Combatant != null)
                {
                    m.Combatant = this; // Pull the target in
                    m.SendMessage("You are pulled into the shadowy embrace of the crab!");
                    m.RevealingAction();
                    Timer.DelayCall(TimeSpan.FromSeconds(10), () => m.RevealingAction()); // Reset visibility after 10 seconds
                }
            }

            m_NextShadowPull = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
        }

        private void DarkSlash()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Crab slashes with dark energy! *");
            PlaySound(0x208);

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Combatant != null && m.Combatant == this)
                {
                    if (m.Skills[SkillName.Magery].Base < 50.0) // Example condition for extra shadow damage
                    {
                        int damage = Utility.RandomMinMax(10, 20);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        m.SendMessage("You are struck by the Shadow Crab's dark slash!");
                    }
                }
            }

            m_NextDarkSlash = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
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
