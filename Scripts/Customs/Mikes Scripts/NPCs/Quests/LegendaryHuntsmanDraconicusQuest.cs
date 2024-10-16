using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class LegendaryHuntsmanDraconicusQuest : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        public override object Title { get { return "The Legendary Huntsman's Challenge: Draconicus"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, valiant hunter! A monstrous beast known as Draconicus The Destroyer " +
                    "has been terrorizing our lands. Your mission is to hunt down and defeat this dreadful " +
                    "creature. I have a magical compass to guide you to its last known location. " +
                    "Be prepared, as Draconicus is incredibly dangerous. Return to me with its defeat, and " +
                    "you will be rewarded with great honors!";
            }
        }

        public override object Refuse { get { return "I understand if you are not ready to face such a challenge."; } }

        public override object Uncomplete { get { return "Draconicus The Destroyer still ravages the land. Have you given up?"; } }

        public override object Complete { get { return "You have done it! Draconicus The Destroyer is no more. You have earned your rewards!"; } }

        private static List<Point3D> SpawnLocations = new List<Point3D>
        {
            new Point3D(1600, 1300, 20), // Felucca
            new Point3D(1450, 1400, 20), // Trammel
            new Point3D(1200, 1000, -25), // Ilshenar
            new Point3D(1050, 450, -30), // Malas
            new Point3D(700, 1300, 25), // Tokuno
            new Point3D(900, 3500, -43) // Ter Mur
            // Add more locations if needed
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

        private DraconicusTheDestroyer m_Draconicus;
        private LegendaryHuntCompassDR m_Compass;

        public LegendaryHuntsmanDraconicusQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DraconicusTheDestroyer), "Draconicus The Destroyer", 1));

            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(HarvestersBlessing), 1, "Harvester's Blessing"));
        }

        public override void OnAccept()
        {
            base.OnAccept();

            int index = Utility.Random(SpawnLocations.Count);
            Point3D location = SpawnLocations[index];
            Map facet = SpawnFacets[index];

            m_Draconicus = new DraconicusTheDestroyer();
            m_Draconicus.MoveToWorld(location, facet);
            m_Draconicus.Home = location;
            m_Draconicus.RangeHome = 10;

            m_Compass = new LegendaryHuntCompassDR();
            m_Compass.TargetLocation = location;
            m_Compass.TargetMap = facet;

            Owner.AddToBackpack(m_Compass);
            Owner.SendMessage("A magical compass has been added to your backpack. Use it to locate Draconicus The Destroyer.");
        }

        public override void OnCompleted()
        {
            Owner.SendMessage("Congratulations! You have defeated Draconicus The Destroyer!");
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

    public class LegendaryHuntCompassDR : Item
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
        public LegendaryHuntCompassDR() : base(0x1878)
        {
            Name = "Legendary Hunt Compass";
            LootType = LootType.Blessed;
        }

        public LegendaryHuntCompassDR(Serial serial) : base(serial) { }

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

    public class LegendaryHuntsmanDraconicus : MondainQuester
    {
        [Constructable]
        public LegendaryHuntsmanDraconicus() : base("Legendary Huntsman Draconicus", "")
        {
        }

        public LegendaryHuntsmanDraconicus(Serial serial) : base(serial)
        {
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[] 
                {
                    typeof(LegendaryHuntsmanDraconicusQuest)
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
