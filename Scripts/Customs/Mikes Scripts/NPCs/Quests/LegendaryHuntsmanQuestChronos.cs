using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class LegendaryHuntsmanQuestChronos : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        public override object Title { get { return "Chronos the TimeLord's Bane"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, intrepid adventurer! The fearsome Chronos The TimeLord has appeared in our world. " +
                    "Your mission is to locate and defeat this formidable foe. " +
                    "To aid you in your quest, I will provide a mystical compass that will guide you to his last known whereabouts. " +
                    "Be vigilant, as Chronos may be elusive. Return to me once he has been vanquished, and you shall be richly rewarded!";
            }
        }

        public override object Refuse { get { return "I see. Perhaps another time."; } }

        public override object Uncomplete { get { return "Chronos still eludes us. Have you lost your way?"; } }

        public override object Complete { get { return "Outstanding! You have defeated Chronos The TimeLord. You have proven yourself a true hero!"; } }

        private static List<Point3D> SpawnLocations = new List<Point3D>
        {
            new Point3D(1584, 1446, 15), // Felucca
            new Point3D(1495, 1582, 15), // Trammel
            new Point3D(1230, 1120, -20), // Ilshenar
            new Point3D(1040, 480, -25), // Malas
            new Point3D(710, 1285, 20), // Tokuno
            new Point3D(900, 3480, -45) // Ter Mur
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

        private ChronosTheTimeLord m_Chronos;
        private LegendaryHuntCompassCH m_Compass;

        public LegendaryHuntsmanQuestChronos() : base()
        {
            AddObjective(new SlayObjective(typeof(ChronosTheTimeLord), "Chronos The TimeLord", 1));

            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(Earthshaker), 1, "Earthshaker"));
        }

        public override void OnAccept()
        {
            base.OnAccept();

            int index = Utility.Random(SpawnLocations.Count);
            Point3D location = SpawnLocations[index];
            Map facet = SpawnFacets[index];

            m_Chronos = new ChronosTheTimeLord();
            m_Chronos.MoveToWorld(location, facet);
            m_Chronos.Home = location;
            m_Chronos.RangeHome = 10;

            m_Compass = new LegendaryHuntCompassCH();
            m_Compass.TargetLocation = location;
            m_Compass.TargetMap = facet;

            Owner.AddToBackpack(m_Compass);
            Owner.SendMessage("A mystical compass has been added to your backpack. Use it to find Chronos The TimeLord.");
        }

        public override void OnCompleted()
        {
            Owner.SendMessage("Congratulations! You have completed the quest and defeated Chronos The TimeLord.");
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

    public class LegendaryHuntCompassCH : Item
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
        public LegendaryHuntCompassCH() : base(0x1878)
        {
            Name = "Legendary Hunt Compass";
            LootType = LootType.Blessed;
        }

        public LegendaryHuntCompassCH(Serial serial) : base(serial) { }

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

    public class LegendaryHuntsmanChronos : MondainQuester
    {
        [Constructable]
        public LegendaryHuntsmanChronos() : base("Legendary Huntsman Chronos", "")
        {
        }

        public LegendaryHuntsmanChronos(Serial serial) : base(serial)
        {
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[] 
                {
                    typeof(LegendaryHuntsmanQuestChronos)
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
