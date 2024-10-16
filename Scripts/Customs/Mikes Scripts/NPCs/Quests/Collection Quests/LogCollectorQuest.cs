using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class LogCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Log Collector's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Gorim, the Woodkeeper. The ancient trees of our land are in great peril. " +
                       "I need your help to gather 500 Logs, which are crucial for the restoration rituals that protect our forests. " +
                       "In return for your valor, I will reward you with gold, a rare Maxxia Scroll, and a special Woodkeeper's Garb. " +
                       "Help us save the woods from decay!";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return with the Logs and I will be grateful for your aid."; } }

        public override object Uncomplete { get { return "I still require 50 Logs to perform the restoration. Return when you have gathered them."; } }

        public override object Complete { get { return "Splendid! You have collected the 50 Logs I needed. Your assistance is vital to the preservation of our forests. " +
                       "Please accept these rewards as a token of my gratitude. May the trees bless your path!"; } }

        public LogCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Log), "Logs", 500, 0x1BDD)); // Assuming Log item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PickpocketsSleekTunic), 1, "Woodkeeper's Garb")); // Assuming Woodkeeper's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Log Collector's Request quest!");
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

    public class WoodkeepersGarb : Item
    {
        [Constructable]
        public WoodkeepersGarb() : base(0x1C05) // Example item ID
        {
            Hue = Utility.Random(1, 3000); // Unique hue for the garb
        }

        public WoodkeepersGarb(Serial serial) : base(serial) { }

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

    public class GorimTheWoodkeeper : MondainQuester
    {
        [Constructable]
        public GorimTheWoodkeeper() : base("The Woodkeeper", "Gorim")
        {
        }

        public GorimTheWoodkeeper(Serial serial) : base(serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Gorim's Forest Robe" });
            AddItem(new ThighBoots { Hue = Utility.Random(1, 3000) });
            AddItem(new WoodenShield { Hue = Utility.Random(1, 3000), Name = "Gorim's Forest Shield" });
            AddItem(new StrawHat { Hue = Utility.Random(1, 3000), Name = "Gorim's Forest Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Gorim's Forest Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Gorim's Woodkeeper Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(LogCollectorQuest)
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
