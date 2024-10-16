using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class DragonScaleCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Dragon Scale Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Drakona, the Keeper of Dragon Lore. Legends speak of the ancient dragons whose scales " +
                       "hold immense power. I require 50 Red Scales to complete an ancient ritual that will reveal forgotten dragon secrets. " +
                       "In return for your heroic efforts, I shall reward you with gold, a rare Maxxia Scroll, and the illustrious Dragonkeeper's Armor.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return with the Red Scales."; } }

        public override object Uncomplete { get { return "I still require 50 Red Scales. Please bring them to me so that I may complete the ritual!"; } }

        public override object Complete { get { return "Wonderful! You have brought me the 50 Red Scales I needed. Your bravery is truly commendable. " +
                       "Accept these rewards as a symbol of my gratitude. May the dragons' wisdom guide your path!"; } }

        public DragonScaleCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(RedScales), "Red Scales", 50, 0x26B4)); // Assuming RedScale item ID is 0xF7F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ScoutsWideBrimHat), 1, "Dragonkeeper's Armor")); // Assuming Dragonkeeper's Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Dragon Scale Collector quest!");
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

    public class DragonKeeperDrakona : MondainQuester
    {
        [Constructable]
        public DragonKeeperDrakona()
            : base("The Keeper of Dragon Lore", "Drakona")
        {
        }

        public DragonKeeperDrakona(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Drakona's Dragon Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new ClothNinjaHood { Hue = Utility.Random(1, 3000), Name = "Drakona's Dragon Hood" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Drakona's Dragon Ring" });
            AddItem(new TribalMask { Hue = Utility.Random(1, 3000), Name = "Drakona's Dragon Mask" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Drakona's Ancient Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DragonScaleCollectorQuest)
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
