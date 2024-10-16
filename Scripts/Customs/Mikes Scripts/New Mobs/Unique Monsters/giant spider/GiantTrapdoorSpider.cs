using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a trapdoor spider corpse")]
    public class GiantTrapdoorSpider : BaseCreature
    {
        private DateTime m_NextBurrow;
        private DateTime m_NextAmbushAttack;
        private DateTime m_NextTrapdoorWeb;
        private DateTime m_NextVenomousBite;
        private DateTime m_NextWebTrap;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        private static readonly int[] m_HueOptions = { 0x5A0, 0x5B1, 0x5B2 };

        [Constructable]
        public GiantTrapdoorSpider()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a trapdoor spider";
            Body = 28;
            Hue = Utility.RandomList(m_HueOptions);
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

        public GiantTrapdoorSpider(Serial serial)
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

        public override FoodType FavoriteFood
        {
            get { return FoodType.Meat; }
        }

        public override PackInstinct PackInstinct
        {
            get { return PackInstinct.Arachnid; }
        }

        public override Poison PoisonImmune
        {
            get { return Poison.Regular; }
        }

        public override Poison HitPoison
        {
            get { return Poison.Regular; }
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
                    m_NextBurrow = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextAmbushAttack = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextTrapdoorWeb = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextVenomousBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextWebTrap = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 5));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBurrow)
                {
                    Burrow();
                }

                if (DateTime.UtcNow >= m_NextAmbushAttack)
                {
                    AmbushAttack();
                }

                if (DateTime.UtcNow >= m_NextTrapdoorWeb)
                {
                    TrapdoorWeb();
                }

                if (DateTime.UtcNow >= m_NextVenomousBite)
                {
                    VenomousBite();
                }

                if (DateTime.UtcNow >= m_NextWebTrap)
                {
                    WebTrap();
                }
            }
        }

        private void Burrow()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The spider burrows underground *");
            Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(() =>
            {
                Point3D newLocation = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
                if (Map.CanSpawnMobile(newLocation))
                {
                    MoveToWorld(newLocation, Map);
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The spider emerges from the ground *");

                    // Optional: Summon smaller spiders on emergence
                    if (Utility.RandomDouble() < 0.3) // 30% chance
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            SmallSpider spiderling = new SmallSpider();
                            spiderling.MoveToWorld(Location, Map);
                        }
                        PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Smaller spiders crawl out! *");
                    }
                }
            }));

            m_NextBurrow = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void AmbushAttack()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The spider strikes from below! *");
            if (Combatant != null)
            {
                int damage = Utility.RandomMinMax(15, 25);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);
                Combatant.PlaySound(0x208);
                ((Mobile)Combatant).SendMessage("You are struck with a venomous bite!");

                // Apply a poison debuff
                ((Mobile)Combatant).ApplyPoison(this, Poison.Greater);
            }

            m_NextAmbushAttack = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void TrapdoorWeb()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The spider spins a trapdoor web! *");
            Point3D trapdoorLocation = new Point3D(X + Utility.RandomMinMax(-2, 2), Y + Utility.RandomMinMax(-2, 2), Z);
            if (Map.CanSpawnMobile(trapdoorLocation))
            {
                TrapdoorWeb trapdoor = new TrapdoorWeb(); // Ensure this class is defined elsewhere
                trapdoor.MoveToWorld(trapdoorLocation, Map);
            }

            m_NextTrapdoorWeb = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void VenomousBite()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The spider bites venomously! *");
                int damage = Utility.RandomMinMax(10, 15);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);
                ((Mobile)Combatant).ApplyPoison(this, Poison.Greater);
                ((Mobile)Combatant).SendMessage("You are afflicted by a venomous bite!");
                Combatant.PlaySound(0x208);
            }

            m_NextVenomousBite = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void WebTrap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The spider sets multiple web traps! *");
            for (int i = 0; i < 3; i++)
            {
                Point3D trapLocation = new Point3D(X + Utility.RandomMinMax(-3, 3), Y + Utility.RandomMinMax(-3, 3), Z);
                if (Map.CanSpawnMobile(trapLocation))
                {
                    TrapdoorWeb trapdoor = new TrapdoorWeb(); // Ensure this class is defined elsewhere
                    trapdoor.MoveToWorld(trapLocation, Map);
                }
            }

            m_NextWebTrap = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(m_NextBurrow);
            writer.Write(m_NextAmbushAttack);
            writer.Write(m_NextTrapdoorWeb);
            writer.Write(m_NextVenomousBite);
            writer.Write(m_NextWebTrap);
            writer.Write(m_AbilitiesInitialized);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextBurrow = reader.ReadDateTime();
            m_NextAmbushAttack = reader.ReadDateTime();
            m_NextTrapdoorWeb = reader.ReadDateTime();
            m_NextVenomousBite = reader.ReadDateTime();
            m_NextWebTrap = reader.ReadDateTime();
            m_AbilitiesInitialized = reader.ReadBool();
        }
    }

    public class SmallSpider : BaseCreature
    {
        [Constructable]
        public SmallSpider()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a small spider";
            Body = 28; // Same as GiantSpider
            Hue = 0x5A0; // Default hue

            SetStr(30, 50);
            SetDex(30, 50);
            SetInt(10, 20);

            SetHits(20, 30);
            SetMana(0);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Poison, 20, 25);

            SetSkill(SkillName.Poisoning, 20.0, 40.0);
            SetSkill(SkillName.MagicResist, 10.0, 20.0);
            SetSkill(SkillName.Tactics, 15.0, 25.0);
            SetSkill(SkillName.Wrestling, 20.0, 30.0);

            Fame = 150;
            Karma = -150;

            VirtualArmor = 8;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 29.1;
        }

        public SmallSpider(Serial serial)
            : base(serial)
        {
        }

        public override FoodType FavoriteFood
        {
            get { return FoodType.Meat; }
        }

        public override PackInstinct PackInstinct
        {
            get { return PackInstinct.Arachnid; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
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

    // Example definition of TrapdoorWeb class
    public class TrapdoorWeb : Item
    {
        [Constructable]
        public TrapdoorWeb()
            : base(0x1BEF) // Use an appropriate item ID
        {
            Movable = false;
            Name = "a trapdoor web";
        }

        public TrapdoorWeb(Serial serial)
            : base(serial)
        {
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.InRange(this.GetWorldLocation(), 1))
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* You are caught in a web trap! *");
                m.SendMessage("You are ensnared in a sticky web!");
                m.Freeze(TimeSpan.FromSeconds(5));
                this.Delete();
            }
        }

        private void StartDeleteTimer()
        {
            Timer.DelayCall(TimeSpan.FromMinutes(1), DeleteTimer); // Adjust the duration as needed
        }

        private void DeleteTimer()
        {
            this.Delete();
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
            StartDeleteTimer();
        }
    }
}
