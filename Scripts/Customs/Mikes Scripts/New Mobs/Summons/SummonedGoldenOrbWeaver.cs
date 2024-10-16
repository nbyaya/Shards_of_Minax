using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a golden orb weaver corpse")]
    public class SummonedGoldenOrbWeaver : BaseCreature
    {
        private DateTime m_NextGoldenWeb;
        private DateTime m_NextWebShield;
        private DateTime m_NextLuminousStrike;
        private bool m_HasWebShield; // Track if the web shield is active
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SummonedGoldenOrbWeaver()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a golden orb weaver";
            Body = 28; // GiantSpider body
            Hue = 1790; // Gold hue
			BaseSoundID = 0x388;

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

            // Initialize ability timers and flag
            m_AbilitiesInitialized = false;
        }

        public SummonedGoldenOrbWeaver(Serial serial) : base(serial) { }

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
                    m_NextGoldenWeb = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                    m_NextWebShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextLuminousStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextGoldenWeb)
                {
                    GoldenWeb();
                }

                if (DateTime.UtcNow >= m_NextWebShield)
                {
                    WebShield();
                }

                if (DateTime.UtcNow >= m_NextLuminousStrike)
                {
                    LuminousStrike();
                }
            }
        }

        private void GoldenWeb()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Spins a radiant web! *");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are caught in the golden web and are slowed!");
                    m.FixedEffect(0x376A, 10, 16);
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        if (!m.Deleted)
                        {
                            int damage = Utility.RandomMinMax(10, 20);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                            m.SendMessage("You continue to burn from the web!");
                            m.SendMessage("Your movement is hindered by the sticky web!");
                            m.Damage(5, this); // Extra persistent damage over time
                        }
                    });

                    m.SendMessage("Your movement speed is reduced!");
                    m.SendMessage("You are slowed by the sticky web!");
                }
            }

            // Reset cooldown with a fixed interval for demonstration
            m_NextGoldenWeb = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void WebShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Creates a web shield! *");
            VirtualArmor += 30;
            m_HasWebShield = true; // Set shield active

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                if (!Deleted)
                {
                    VirtualArmor -= 30;
                    m_HasWebShield = false; // Set shield inactive
                }
            });

            // Reset cooldown with a fixed interval for demonstration
            m_NextWebShield = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void LuminousStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Unleashes a blinding attack! *");
            Effects.SendLocationEffect(Location, Map, 0x3728, 10, 16, 0xF8, 0); // Light effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are blinded by the luminous strike!");
                    m.FixedEffect(0x376A, 10, 16);
                    m.SendMessage("You are struck by a burst of radiant energy!");

                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        if (!m.Deleted)
                        {
                            int damage = Utility.RandomMinMax(15, 25);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        }
                    });
                }
            }

            // Reset cooldown with a fixed interval for demonstration
            m_NextLuminousStrike = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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

            // Reset initialization flag on deserialization
            m_AbilitiesInitialized = false;
        }
    }
}
