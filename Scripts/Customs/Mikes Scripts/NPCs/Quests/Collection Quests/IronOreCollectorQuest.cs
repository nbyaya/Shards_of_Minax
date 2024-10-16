using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class IronOreCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Iron Ore Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Brondar, the Master Blacksmith. The heart of my forge beats with a need for iron—" +
                       "specifically, 50 Iron Ores. My legendary forge has been dormant, its power diminished, and only these ores can " +
                       "restore it to its former glory. Assist me, and in return, you will earn gold, a rare Maxxia Scroll, and the " +
                       "Master Blacksmith's Cap, a testament to your dedication.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return with the Iron Ores and we shall discuss further."; } }

        public override object Uncomplete { get { return "I still require 50 Iron Ores. Return to me once you have gathered them all, so we can reinvigorate the forge!"; } }

        public override object Complete { get { return "Marvelous! You have delivered the 50 Iron Ores needed. The forge shall roar once more thanks to your efforts. " +
                       "Please accept these rewards as a symbol of my gratitude. May your journeys be filled with fortune and glory!"; } }

        public IronOreCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(IronOre), "Iron Ores", 50, 0x19B9)); // Assuming Iron Ore item ID is 0x19B9
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PreserversCap), 1, "Master Blacksmith's Cap")); // Assuming Master Blacksmith's Apron is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Iron Ore Collector quest!");
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

    public class MasterBlacksmithBrondar : MondainQuester
    {
        [Constructable]
        public MasterBlacksmithBrondar()
            : base("The Master Blacksmith", "Brondar")
        {
        }

        public MasterBlacksmithBrondar(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2049; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Brondar's Forged Plate" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateArms { Hue = Utility.Random(1, 3000) });
            AddItem(new SmithHammer { Hue = Utility.Random(1, 3000), Name = "Brondar's Hammer of Crafting" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Brondar's Precision Cloak" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Brondar's Crafting Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(IronOreCollectorQuest)
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
