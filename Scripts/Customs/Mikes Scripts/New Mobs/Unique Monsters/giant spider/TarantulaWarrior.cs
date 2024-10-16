using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a tarantula warrior corpse")]
    public class TarantulaWarrior : GiantSpider
    {
        private DateTime m_NextVenomousBite;
        private DateTime m_NextWebTrap;
        private DateTime m_NextFearsomeRoar;
        private DateTime m_NextSummonSpiders;
        private DateTime m_NextEnrage;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public TarantulaWarrior()
            : base()
        {
            Name = "a Tarantula Warrior";
            Hue = 1782; // Unique hue
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
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public TarantulaWarrior(Serial serial)
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
                    m_NextVenomousBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextWebTrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFearsomeRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSummonSpiders = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_NextEnrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVenomousBite)
                {
                    UseVenomousBite();
                }

                if (DateTime.UtcNow >= m_NextWebTrap)
                {
                    UseWebTrap();
                }

                if (DateTime.UtcNow >= m_NextFearsomeRoar)
                {
                    UseFearsomeRoar();
                }

                if (DateTime.UtcNow >= m_NextSummonSpiders)
                {
                    SummonSpiders();
                }

                if (DateTime.UtcNow >= m_NextEnrage)
                {
                    Enrage();
                }
            }
        }

        private void UseVenomousBite()
        {
            if (Combatant == null) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tarantula Warrior bites with venomous fangs! *");
            PlaySound(0x2A3);

            if (Combatant is PlayerMobile player)
            {
                player.SendMessage("You are bitten by the Tarantula Warrior's venomous fangs!");
                AOS.Damage(Combatant, this, 15, 0, 100, 0, 0, 0); // Poison damage
                Timer.DelayCall(TimeSpan.FromSeconds(1), () => AOS.Damage(Combatant, this, 15, 0, 100, 0, 0, 0)); // More poison damage
            }

            m_NextVenomousBite = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        private void UseWebTrap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tarantula Warrior spins multiple web traps! *");
            PlaySound(0x20F);

            for (int i = 0; i < 3; i++)
            {
                Point3D trapLocation = new Point3D(
                    X + Utility.RandomMinMax(-5, 5),
                    Y + Utility.RandomMinMax(-5, 5),
                    Z
                );

                TrapWeb trapWeb = new TrapWeb();
                trapWeb.MoveToWorld(trapLocation, Map);
            }

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m is PlayerMobile player)
                {
                    player.SendMessage("You are ensnared by the web traps!");
                    player.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextWebTrap = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void UseFearsomeRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tarantula Warrior emits a fearsome roar! *");
            PlaySound(0x60D);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m is PlayerMobile player)
                {
                    player.SendMessage("The roar of the Tarantula Warrior fills you with fear!");
                    player.SendMessage("You feel disoriented and less accurate!");
                    player.Damage(5, this); // Reduces morale
                    player.SendMessage("You are momentarily disoriented!");
                    // Additional disorientation effect could be implemented here
                }
            }

            m_NextFearsomeRoar = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void SummonSpiders()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tarantula Warrior summons additional spiders! *");
            PlaySound(0x2A3);

            for (int i = 0; i < 3; i++)
            {
                Point3D spawnLocation = new Point3D(
                    X + Utility.RandomMinMax(-3, 3),
                    Y + Utility.RandomMinMax(-3, 3),
                    Z
                );

                if (Map.CanSpawnMobile(spawnLocation))
                {
                    SpiderlingFlea spider = new SpiderlingFlea();
                    spider.MoveToWorld(spawnLocation, Map);
                    spider.Combatant = Combatant;
                }
            }

            m_NextSummonSpiders = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void Enrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Tarantula Warrior becomes enraged! *");
            PlaySound(0x2A3);

            SetDamage(20, 30);
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Poison, 70, 80);

            m_NextEnrage = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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

    public class SpiderlingFlea : GiantSpider
    {
        [Constructable]
        public SpiderlingFlea() : base()
        {
            Name = "a spiderling";
            Hue = 2254; // Consistent hue
            SetStr(30);
            SetDex(30);
            SetInt(20);

            SetHits(20);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Poison, 30, 40);

            SetSkill(SkillName.MagicResist, 30.0, 50.0);
            SetSkill(SkillName.Tactics, 40.0, 60.0);
            SetSkill(SkillName.Wrestling, 40.0, 60.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 10;
        }

        public SpiderlingFlea(Serial serial) : base(serial)
        {
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
        }
    }
}
