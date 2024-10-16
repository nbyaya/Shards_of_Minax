using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class RawRibCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Hunt for Raw Ribs"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Gryndor, a seasoned hunter with a passion for the wilderness. " +
                       "A troubling curse has befallen our land, and I need your assistance to gather 50 Raw Ribs. " +
                       "These ribs are essential for a ritual that will help us dispel the curse. In return for your bravery, " +
                       "I will reward you with gold, a rare Maxxia Scroll, and a unique Hunter's Garb that bears the mark of our quest.";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, return to me with the Raw Ribs."; } }

        public override object Uncomplete { get { return "I still need 50 Raw Ribs to continue with the ritual. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Fantastic work! You have collected the 50 Raw Ribs needed for the ritual. " +
                       "Your courage and dedication are commendable. Accept these rewards as a token of my gratitude. " +
                       "May the winds be ever in your favor!"; } }

        public RawRibCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(RawRibs), "Raw Ribs", 50, 0x9F1)); // Assuming Raw Ribs item ID is 0x1F6
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PlateLeggingsOfCommand), 1, "Hunter's Garb")); // Assuming Hunter's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Hunt for Raw Ribs quest!");
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

    public class GryndorTheHunter : MondainQuester
    {
        [Constructable]
        public GryndorTheHunter()
            : base("The Hunter", "Gryndor")
        {
        }

        public GryndorTheHunter(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest { Hue = Utility.Random(1, 3000), Name = "Gryndor's Hunter Vest" });
            AddItem(new StuddedLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Gryndor's Hunter Gloves" });
            AddItem(new ThighBoots { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Gryndor's Feathered Hat" });
            AddItem(new Bow { Hue = Utility.Random(1, 3000), Name = "Gryndor's Longbow" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Gryndor's Hunting Pack";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(RawRibCollectorQuest)
                };
            }
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
