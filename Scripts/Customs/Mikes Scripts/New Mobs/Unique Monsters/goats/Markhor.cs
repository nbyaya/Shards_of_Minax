using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a markhor corpse")]
    public class Markhor : BaseCreature
    {
        private DateTime m_NextHornTwister;
        private DateTime m_NextShaggyShield;
        private DateTime m_NextRagingCharge;
        private bool m_AbilitiesInitialized;
        private bool m_IsCharging;

        [Constructable]
        public Markhor()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Markhor";
            Body = 0xD1; // Goat body
            Hue = 1908; // Reddish-brown hue
            BaseSoundID = 0x99;

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

            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public Markhor(Serial serial)
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
                    m_NextHornTwister = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextShaggyShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextRagingCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to true after initialization
                }

                if (DateTime.UtcNow >= m_NextHornTwister)
                {
                    HornTwister();
                }

                if (DateTime.UtcNow >= m_NextShaggyShield)
                {
                    ShaggyShield();
                }

                if (DateTime.UtcNow >= m_NextRagingCharge && !m_IsCharging)
                {
                    RagingCharge();
                }
            }
        }

        private void HornTwister()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Markhor twists its horns, confusing its enemies!*");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are disoriented by the Markhor's horn twist!");
                    // Logic to reduce attack accuracy or apply a debuff would go here
                }
            }

            Random rand = new Random();
            m_NextHornTwister = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40)); // Randomize cooldown
        }

        private void ShaggyShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Markhor's shaggy coat shimmers with protective energy!*");
            
            // Add a visual effect to show the shield
            FixedEffect(0x376A, 10, 16);

            // Apply temporary damage reduction
            this.VirtualArmor += 20; // Increase armor temporarily
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => this.VirtualArmor -= 20);

            Random rand = new Random();
            m_NextShaggyShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60)); // Randomize cooldown
        }

        private void RagingCharge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Markhor charges with a fierce rage!*");
            m_IsCharging = true;
            this.PlaySound(0x5F2); // Charging sound

            // Charge effect
            Effects.SendLocationEffect(Location, Map, 0x1F5, 10, 10); // Visual effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The Markhor's charge slams into you with great force!");
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(3), () => m_IsCharging = false);

            Random rand = new Random();
            m_NextRagingCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(60, 120)); // Randomize cooldown
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

            m_AbilitiesInitialized = false; // Reset initialization flag
            m_NextHornTwister = DateTime.UtcNow;
            m_NextShaggyShield = DateTime.UtcNow;
            m_NextRagingCharge = DateTime.UtcNow;
        }
    }
}
