using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class LegendaryHuntsmanSDQuest : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        public override object Title { get { return "The Legendary Super Dragon Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, brave hunter! A legendary Super Dragon has been spotted in our realm. " +
                    "Your task is to track down this fearsome creature and slay it. " +
                    "I will provide you with a magical compass that will guide you to its last known location. " +
                    "Be warned, the Super Dragon may move from this spot, so stay alert and use your tracking skills. " +
                    "Return to me once you have slain the beast, and great rewards shall be yours!";
            }
        }

        public override object Refuse { get { return "I understand. Not everyone is cut out for such a dangerous hunt."; } }

        public override object Uncomplete { get { return "The Super Dragon still roams our lands. Have you lost your nerve?"; } }

        public override object Complete { get { return "Incredible! You've slain the legendary Super Dragon. Your name shall be remembered among the greatest of hunters!"; } }

        private static List<Point3D> SpawnLocations = new List<Point3D>
        {
            new Point3D(1553, 1426, 20), // Felucca
            new Point3D(1475, 1513, 20), // Trammel
            new Point3D(1220, 1098, -25), // Ilshenar
            new Point3D(1026, 497, -30), // Malas
            new Point3D(701, 1276, 25), // Tokuno
            new Point3D(898, 3479, -43) // Ter Mur
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

        private SuperDragon m_SuperDragon;
        private LegendaryHuntCompassSD m_Compass;

        public LegendaryHuntsmanSDQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SuperDragon), "Legendary Super Dragon", 1));

            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WhisperingWind), 1, "Whispering Wind"));
        }

        public override void OnAccept()
        {
            base.OnAccept();

            int index = Utility.Random(SpawnLocations.Count);
            Point3D location = SpawnLocations[index];
            Map facet = SpawnFacets[index];

            m_SuperDragon = new SuperDragon();
            m_SuperDragon.MoveToWorld(location, facet);
            m_SuperDragon.Home = location;
            m_SuperDragon.RangeHome = 10;

            m_Compass = new LegendaryHuntCompassSD();
            m_Compass.TargetLocation = location;
            m_Compass.TargetMap = facet;

            Owner.AddToBackpack(m_Compass);
            Owner.SendMessage("A magical compass has been added to your backpack. Use it to locate the Super Dragon.");
        }

        public override void OnCompleted()
        {
            Owner.SendMessage("You have completed the Legendary Super Dragon Hunt!");
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

    public class LegendaryHuntCompassSD : Item
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
        public LegendaryHuntCompassSD() : base(0x1878)
        {
            Name = "Legendary Hunt Compass";
            LootType = LootType.Blessed;
        }

        public LegendaryHuntCompassSD(Serial serial) : base(serial) { }

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

    public class LegendaryHuntsmanSD : MondainQuester
    {
        [Constructable]
        public LegendaryHuntsmanSD() : base("Legendary Super Dragon Hunter", "")
        {
        }

        public LegendaryHuntsmanSD(Serial serial) : base(serial)
        {
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[] 
                {
                    typeof(LegendaryHuntsmanSDQuest)
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
