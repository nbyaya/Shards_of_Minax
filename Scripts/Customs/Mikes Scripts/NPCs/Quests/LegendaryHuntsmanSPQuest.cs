using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class LegendaryHuntsmanSPQuest : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        public override object Title { get { return "The Legendary Slime Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, fearless adventurer! A legendary creature known as the Slime Princess Suiblex has been spotted in our realm. " +
                    "Your mission is to track down this elusive beast and defeat it. " +
                    "I will provide you with a magical compass to aid in your search. " +
                    "The Slime Princess Suiblex may change locations, so keep your wits about you and use your compass wisely. " +
                    "Return to me once you have vanquished the monster, and you shall be richly rewarded!";
            }
        }

        public override object Refuse { get { return "Very well. Perhaps another time, when you are more prepared."; } }

        public override object Uncomplete { get { return "The Slime Princess Suiblex still eludes us. Have you given up?"; } }

        public override object Complete { get { return "Marvelous! You have slain the legendary Slime Princess Suiblex. You are truly a hero among hunters!"; } }

        private static List<Point3D> SpawnLocations = new List<Point3D>
        {
            new Point3D(1620, 1520, 30), // Felucca
            new Point3D(1550, 1400, 30), // Trammel
            new Point3D(1200, 1200, 15), // Ilshenar
            new Point3D(1050, 500, 10), // Malas
            new Point3D(700, 1300, 25), // Tokuno
            new Point3D(900, 3500, 20) // Ter Mur
            // Add more locations as needed
        };

        private static List<Map> SpawnFacets = new List<Map>
        {
            Map.Felucca,
            Map.Trammel,
            Map.Ilshenar,
            Map.Malas,
            Map.Tokuno,
            Map.TerMur
            // Ensure this list matches the SpawnLocations list
        };

        private SlimePrincessSuiblex m_SlimePrincessSuiblex;
        private LegendaryHuntCompassSP m_Compass;

        public LegendaryHuntsmanSPQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SlimePrincessSuiblex), "Legendary Slime Princess Suiblex", 1));

            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(CrimsonCleaver), 1, "Crimson Cleaver"));
        }

        public override void OnAccept()
        {
            base.OnAccept();

            int index = Utility.Random(SpawnLocations.Count);
            Point3D location = SpawnLocations[index];
            Map facet = SpawnFacets[index];

            m_SlimePrincessSuiblex = new SlimePrincessSuiblex();
            m_SlimePrincessSuiblex.MoveToWorld(location, facet);
            m_SlimePrincessSuiblex.Home = location;
            m_SlimePrincessSuiblex.RangeHome = 10;

            m_Compass = new LegendaryHuntCompassSP();
            m_Compass.TargetLocation = location;
            m_Compass.TargetMap = facet;

            Owner.AddToBackpack(m_Compass);
            Owner.SendMessage("A magical compass has been added to your backpack. Use it to locate the Slime Princess Suiblex.");
        }

        public override void OnCompleted()
        {
            Owner.SendMessage("You have completed the Legendary Slime Hunt!");
            Owner.PlaySound(CompleteSound);
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

    public class LegendaryHuntCompassSP : Item
    {
        private Point3D m_TargetLocation;
        private Map m_TargetMap;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D TargetLocation
        {
            get { return m_TargetLocation; }
            set { m_TargetLocation = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map TargetMap
        {
            get { return m_TargetMap; }
            set { m_TargetMap = value; }
        }

        [Constructable]
        public LegendaryHuntCompassSP() : base(0x1878)
        {
            Name = "Legendary Hunt Compass";
            LootType = LootType.Blessed;
        }

        public LegendaryHuntCompassSP(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendMessage(String.Format("The compass points to: {0} ({1}, {2}, {3})", 
                m_TargetMap, m_TargetLocation.X, m_TargetLocation.Y, m_TargetLocation.Z));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_TargetLocation);
            writer.Write(m_TargetMap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_TargetLocation = reader.ReadPoint3D();
            m_TargetMap = reader.ReadMap();
        }
    }

    public class LegendaryHuntsmanSP : MondainQuester
    {
        [Constructable]
        public LegendaryHuntsmanSP() : base("Legendary Slime Hunter", "")
        {
        }

        public LegendaryHuntsmanSP(Serial serial) : base(serial)
        {
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[] 
                {
                    typeof(LegendaryHuntsmanSPQuest)
                };
            }
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
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
