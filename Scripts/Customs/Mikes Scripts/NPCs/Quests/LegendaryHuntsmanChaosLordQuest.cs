using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class LegendaryHuntsmanChaosLordQuest : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        public override object Title { get { return "The Legendary Huntsman's Chaos Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, intrepid adventurer! A Chaos Lord has been sighted in our world. " +
                    "Your mission is to hunt down and defeat this formidable creature. " +
                    "I will provide you with a mystical compass to aid you in tracking it. " +
                    "Be vigilant, as the Chaos Lord may be elusive and move around. " +
                    "Return to me after slaying the beast, and you will be rewarded with great treasures!";
            }
        }

        public override object Refuse { get { return "I see. Perhaps you are not ready for such a challenge."; } }

        public override object Uncomplete { get { return "The Chaos Lord still evades capture. Have you lost your courage?"; } }

        public override object Complete { get { return "Amazing! You've vanquished the Chaos Lord. You have earned your place among the legendary hunters!"; } }

        private static List<Point3D> SpawnLocations = new List<Point3D>
        {
            new Point3D(1234, 567, 10), // Example locations
            new Point3D(789, 1011, 20),
            new Point3D(1213, 1415, 30),
            new Point3D(1617, 1819, 40),
            new Point3D(2021, 2223, 50),
            new Point3D(2425, 2627, 60)
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

        private ChaosLord m_ChaosLord;
        private LegendaryHuntCompassCL m_Compass;

        public LegendaryHuntsmanChaosLordQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ChaosLord), "Chaos Lord", 1));

            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SerpentsFung), 1, "Serpents Fung"));
        }

        public override void OnAccept()
        {
            base.OnAccept();

            int index = Utility.Random(SpawnLocations.Count);
            Point3D location = SpawnLocations[index];
            Map facet = SpawnFacets[index];

            m_ChaosLord = new ChaosLord();
            m_ChaosLord.MoveToWorld(location, facet);
            m_ChaosLord.Home = location;
            m_ChaosLord.RangeHome = 10;

            m_Compass = new LegendaryHuntCompassCL();
            m_Compass.TargetLocation = location;
            m_Compass.TargetMap = facet;

            Owner.AddToBackpack(m_Compass);
            Owner.SendMessage("A mystical compass has been added to your backpack. Use it to find the Chaos Lord.");
        }

        public override void OnCompleted()
        {
            Owner.SendMessage("Congratulations! You have successfully completed the Legendary Huntsman's Chaos Challenge!");
            Owner.PlaySound(0x1F5); // Adjust sound if necessary
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

    public class LegendaryHuntCompassCL : Item
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
        public LegendaryHuntCompassCL() : base(0x1878)
        {
            Name = "Legendary Hunt Compass";
            LootType = LootType.Blessed;
        }

        public LegendaryHuntCompassCL(Serial serial) : base(serial) { }

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

    public class LegendaryHuntsmanChaosLord : MondainQuester
    {
        [Constructable]
        public LegendaryHuntsmanChaosLord() : base("Legendary Huntsman Chaos", "")
        {
        }

        public LegendaryHuntsmanChaosLord(Serial serial) : base(serial)
        {
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[] 
                {
                    typeof(LegendaryHuntsmanChaosLordQuest)
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
