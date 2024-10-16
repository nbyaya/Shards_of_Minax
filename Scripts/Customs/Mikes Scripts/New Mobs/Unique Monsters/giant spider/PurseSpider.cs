using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a purse spider corpse")]
    public class PurseSpider : BaseCreature
    {
        private DateTime m_NextBagOfTricks;
        private DateTime m_NextHiddenWeb;
        private DateTime m_NextWebSiphon;
        private DateTime m_NextWebSummon;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public PurseSpider() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a purse spider";
            Body = 28; // Giant Spider body
            Hue = 1787; // Unique hue for the Purse Spider
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

        public PurseSpider(Serial serial) : base(serial)
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
                    m_NextBagOfTricks = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextHiddenWeb = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextWebSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_NextWebSummon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBagOfTricks)
                {
                    BagOfTricks();
                }

                if (DateTime.UtcNow >= m_NextHiddenWeb)
                {
                    HiddenWeb();
                }

                if (DateTime.UtcNow >= m_NextWebSiphon)
                {
                    WebSiphon();
                }

                if (DateTime.UtcNow >= m_NextWebSummon)
                {
                    WebSummon();
                }
            }
        }

        private void BagOfTricks()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Throws a sticky web! *");
            Effects.SendLocationEffect(Location, Map, 0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You feel disoriented as the sticky web surrounds you!");
                    m.Freeze(TimeSpan.FromSeconds(4));
                    m.SendMessage("You are confused by the web!");
                }
            }

            m_NextBagOfTricks = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for BagOfTricks
        }

        private void HiddenWeb()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sets a hidden web trap! *");
            Effects.SendLocationEffect(Location, Map, 0x374A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are caught in a hidden web trap!");
                    m.Freeze(TimeSpan.FromSeconds(6));
                }
            }

            m_NextHiddenWeb = DateTime.UtcNow + TimeSpan.FromSeconds(35); // Cooldown for HiddenWeb
        }

        private void WebSiphon()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Siphons health from enemies! *");
            Effects.SendLocationEffect(Location, Map, 0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int siphonedHealth = Utility.RandomMinMax(15, 25);
                    m.SendMessage("The spider drains your life force!");
                    this.Heal(siphonedHealth);
                    AOS.Damage(m, this, siphonedHealth, 0, 0, 100, 0, 0);
                }
            }

            m_NextWebSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for WebSiphon
        }

        private void WebSummon()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summons additional spiders! *");
            Effects.SendLocationEffect(Location, Map, 0x373A, 10, 16);

            for (int i = 0; i < 2; i++)
            {
                Point3D spawnLocation = GetSpawnPosition(3);
                if (spawnLocation != Point3D.Zero)
                {
                    PurseSpiderling spiderling = new PurseSpiderling();
                    spiderling.MoveToWorld(spawnLocation, Map);
                    spiderling.Combatant = this;
                }
            }

            m_NextWebSummon = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for WebSummon
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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

            // Reset the ability timers and flag when deserializing
            m_AbilitiesInitialized = false;
            m_NextBagOfTricks = DateTime.UtcNow;
            m_NextHiddenWeb = DateTime.UtcNow;
            m_NextWebSiphon = DateTime.UtcNow;
            m_NextWebSummon = DateTime.UtcNow;
        }
    }

    public class PurseSpiderling : BaseCreature
    {
        [Constructable]
        public PurseSpiderling() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a purse spiderling";
            Body = 28; // Small Spider body
            Hue = 1153; // Same hue as the Purse Spider

            SetStr(30, 50);
            SetDex(40, 60);
            SetInt(10, 20);

            SetHits(20, 40);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Poison, 40, 50);

            SetSkill(SkillName.Poisoning, 40.1, 60.0);
            SetSkill(SkillName.MagicResist, 20.1, 40.0);
            SetSkill(SkillName.Tactics, 30.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 60.0);

            Fame = 200;
            Karma = -200;

            VirtualArmor = 10;
        }

        public PurseSpiderling(Serial serial) : base(serial)
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
