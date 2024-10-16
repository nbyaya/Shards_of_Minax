using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class ActsOfHumilityQuest : BaseQuest
    {
        public override bool DoneOnce { get { return false; } } // Repeatable
        public override object Title { get { return "Acts of Humility"; } }
        public override object Description
        {
            get
            {
                return "Greetings, traveler. In this land, arrogance has taken root, and only through acts of humility can it be quelled. " +
                       "Your mission is to either collect humble offerings or vanquish creatures of arrogance. Return to me once your task is done, and you shall be rewarded.";
            }
        }

        public override object Refuse { get { return "Humility is not easy to embrace. Come back if you change your mind."; } }

        public override object Uncomplete { get { return "Arrogance still plagues the land. Please complete your task."; } }

        public override object Complete { get { return "You have demonstrated true humility. Here is your reward."; } }

        private static List<Point3D> SpawnLocations = new List<Point3D>
        {
            new Point3D(1262, 504, -4),
            new Point3D(1304, 423, 31),
            new Point3D(1317, 340, 1),
            new Point3D(1389, 302, 32),
            new Point3D(1444, 246, 16),
            new Point3D(1510, 236, -13),
            new Point3D(1497, 288, -8),
            new Point3D(691, 1350, -58),			
            new Point3D(736, 1231, -58),
            new Point3D(680, 1224, -101),
            new Point3D(714, 1136, -81),
            new Point3D(620, 1152, -74),			
            new Point3D(490, 1211, -63),
            new Point3D(353, 1232, -41),
            new Point3D(379, 1099, -65),
            new Point3D(437, 1007, -91),			
            new Point3D(506, 979, -86),
            new Point3D(645, 1023, -83),
            new Point3D(686, 849, -67),
            new Point3D(730, 523, -54),			
            new Point3D(644, 450, -75),
            new Point3D(758, 341, -43),
            new Point3D(652, 297, -50),
            new Point3D(472, 415, -64),			
            new Point3D(387, 330, -50),
            new Point3D(285, 302, -55),
            new Point3D(692, 745, -34),
            new Point3D(1087, 1104, -30),			
            new Point3D(1195, 1238, -19),
            new Point3D(1272, 1292, -21),
            new Point3D(1410, 1249, -16),
            new Point3D(1563, 1231, -18),
            new Point3D(1671, 1134, -8),
            new Point3D(1649, 1028, -20),
            new Point3D(1554, 993, 3),
            new Point3D(1449, 986, -24)	
        };

        private static List<Map> SpawnMaps = new List<Map>
        {
            Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
			Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
			Map.Ilshenar,
            Map.Ilshenar,
			Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
			Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
			Map.Ilshenar,
            Map.Ilshenar,
			Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
			Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
			Map.Ilshenar,
            Map.Ilshenar,
			Map.Ilshenar,
			Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar,
			Map.Ilshenar,
            Map.Ilshenar,
            Map.Ilshenar
        };

        private Mobile m_Target;
        private Item m_QuestItem;

        public ActsOfHumilityQuest() : base()
        {
            // Randomly decide between kill or collect objective
            if (Utility.RandomBool())
            {
                // Kill Objective
                AddObjective(new SlayObjective(typeof(CreatureOfArrogance), "Creature of Arrogance", 1));
            }
            else
            {
                // Collection Objective
                AddObjective(new ObtainObjective(typeof(HumbleOffering), "Humble Offerings", 5, 0x14F0)); // Example item ID
            }

            // Reward: Humility Stone
            AddReward(new BaseReward(typeof(HumilityStone), 1, "1 Humility Stone"));
            AddReward(new BaseReward(typeof(Gold), 2000, "2000 Gold"));
        }

        public override void OnAccept()
        {
            base.OnAccept();

            int index = Utility.Random(SpawnLocations.Count);
            Point3D location = SpawnLocations[index];
            Map map = SpawnMaps[index];

            string objectiveMessage = "";

            // Dynamically spawn target or quest item based on objective
            if (Objectives[0] is SlayObjective)
            {
                // Spawn a Creature of Arrogance
                m_Target = new CreatureOfArrogance();
                m_Target.MoveToWorld(location, map);

                objectiveMessage = "A Creature of Arrogance has been seen near " + location + " on the Ilshenar map. Defeat it to prove your humility.";
                Owner.SendMessage(objectiveMessage);
            }
            else if (Objectives[0] is ObtainObjective)
            {
                // Spawn Humble Offerings
                SpawnHumbleOfferings(location, map);

                objectiveMessage = "Humble Offerings are scattered around " + location + " on the Ilshenar map. Collect them as a sign of humility.";
                Owner.SendMessage(objectiveMessage);
            }

            // Give the player a note with the location of the quest objectives
            GiveSimpleNoteToPlayer(objectiveMessage);
        }

        private void SpawnHumbleOfferings(Point3D location, Map map)
        {
            for (int i = 0; i < 5; i++) // Adjust the number if you want to spawn more items
            {
                Item offering = new HumbleOffering();
                offering.MoveToWorld(location, map);
            }
        }

        private void GiveSimpleNoteToPlayer(string message)
        {
            SimpleNote note = new SimpleNote();
            note.Name = "Objective Location";
            note.NoteMessage = message;
            Owner.AddToBackpack(note);
        }

        public override void OnCompleted()
        {
            base.OnCompleted();
            Owner.SendMessage("Your acts of humility have made an impact.");
            Owner.PlaySound(0x1F5); // Optional sound effect
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
