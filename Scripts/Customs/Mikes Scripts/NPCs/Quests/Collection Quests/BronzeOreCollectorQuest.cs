using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BronzeOreCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Bronze Ore Conundrum"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave explorer! I am Thrain, the Forgemaster. The ancient forge has been silent for too long due to " +
                       "a lack of Bronze Ore. These ores are the key to rekindling the forge’s power and crafting legendary items. " +
                       "I need you to bring me 50 Bronze Ore. In return, I shall reward you with gold, a rare Maxxia Scroll, and a unique " +
                       "Forgemaster's Armor, which glows with the power of the forge itself.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Bronze Ore."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Bronze Ore. Please bring them to me as soon as you can."; } }

        public override object Complete { get { return "Ah, excellent work! You have gathered the 50 Bronze Ore I required. Your assistance is invaluable. " +
                       "Please accept these rewards as a token of my appreciation. May your adventures be as enduring as the forge's flames!"; } }

        public BronzeOreCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BronzeOre), "Bronze Ore", 50, 0x19B9)); // Assuming Bronze Ore item ID is 0x19B9
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WarlordsGorget), 1, "Forgemaster's Gorget")); // Assuming Forgemaster's Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Bronze Ore Conundrum quest!");
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

    public class ForgemasterThrain : MondainQuester
    {
        [Constructable]
        public ForgemasterThrain()
            : base("The Forgemaster", "Thrain")
        {
        }

        public ForgemasterThrain(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 100, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Forgemaster's Chestplate" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Forgemaster's Helm" });
            AddItem(new Boots { Hue = Utility.Random(1, 3000) });
            AddItem(new SmithHammer { Hue = Utility.Random(1, 3000), Name = "Thrain's Smithing Hammer" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Forgemaster's Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BronzeOreCollectorQuest)
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
