using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class LegendaryHuntsmanSEQuest : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        public override object Title { get { return "The Legendary Searing Exarch's Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, fearless adventurer! The legendary Searing Exarch has been sighted in our realm. " +
                    "Your mission is to track down and defeat this formidable creature. " +
                    "I will give you a magical compass to guide you to its last known location. " +
                    "Be vigilant, as the Searing Exarch may move from this spot. Use your tracking skills wisely. " +
                    "Return to me after slaying the beast to claim your rewards!";
            }
        }

        public override object Refuse { get { return "I understand. Not everyone is ready for such a perilous hunt."; } }

        public override object Uncomplete { get { return "The Searing Exarch still lurks in our lands. Have you lost your resolve?"; } }

        public override object Complete { get { return "Fantastic! You've defeated the legendary Searing Exarch. Your bravery will be remembered!"; } }

        private static List<Point3D> SpawnLocations = new List<Point3D>
        {
            new Point3D(2000, 1500, 30), // Felucca
            new Point3D(2200, 1800, 30), // Trammel
            new Point3D(1500, 1200, 15), // Ilshenar
            new Point3D(1300, 1000, 10), // Malas
            new Point3D(800, 1600, 25), // Tokuno
            new Point3D(1100, 3300, -30) // Ter Mur
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

        private SearingExarch m_SearingExarch;
        private LegendaryHuntCompassSE m_Compass;

        public LegendaryHuntsmanSEQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SearingExarch), "Legendary Searing Exarch", 1));

            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ReapersToll), 1, "Reaper's Toll"));
        }

        public override void OnAccept()
        {
            base.OnAccept();

            int index = Utility.Random(SpawnLocations.Count);
            Point3D location = SpawnLocations[index];
            Map facet = SpawnFacets[index];

            m_SearingExarch = new SearingExarch();
            m_SearingExarch.MoveToWorld(location, facet);
            m_SearingExarch.Home = location;
            m_SearingExarch.RangeHome = 10;

            m_Compass = new LegendaryHuntCompassSE();
            m_Compass.TargetLocation = location;
            m_Compass.TargetMap = facet;

            Owner.AddToBackpack(m_Compass);
            Owner.SendMessage("A magical compass has been added to your backpack. Use it to find the Searing Exarch.");
        }

        public override void OnCompleted()
        {
            Owner.SendMessage("Congratulations on completing the Legendary Searing Exarch's Challenge!");
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

    public class LegendaryHuntCompassSE : Item
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
        public LegendaryHuntCompassSE() : base(0x1878)
        {
            Name = "Legendary Hunt Compass";
            LootType = LootType.Blessed;
        }

        public LegendaryHuntCompassSE(Serial serial) : base(serial) { }

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

    public class LegendaryHuntsmanSE : MondainQuester
    {
        [Constructable]
        public LegendaryHuntsmanSE() : base("Legendary Searing Exarch Hunter", "")
        {
        }

        public LegendaryHuntsmanSE(Serial serial) : base(serial)
        {
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[] 
                {
                    typeof(LegendaryHuntsmanSEQuest)
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
