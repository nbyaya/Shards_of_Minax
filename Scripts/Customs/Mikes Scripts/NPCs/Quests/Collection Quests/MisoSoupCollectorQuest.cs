using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MisoSoupCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Miso Soup Mastery"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave soul! I am Tetsuya, the Grand Chef of the Celestial Kitchen. My culinary skills are renowned, " +
                       "but I am in desperate need of 50 MisoSoups. These soups are more than just food; they hold the essence of ancient " +
                       "recipes passed down through generations. Completing this task will grant you a valuable reward including gold, " +
                       "a rare Maxxia Scroll, and an exquisite outfit befitting a master chef.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return with the MisoSoups."; } }

        public override object Uncomplete { get { return "I still need 50 MisoSoups. Your help in gathering them would be greatly appreciated!"; } }

        public override object Complete { get { return "Wonderful! You've brought me the 50 MisoSoups I needed. Your effort is greatly appreciated. " +
                       "Accept these rewards as a token of my gratitude. May your culinary adventures be as splendid as my soups!"; } }

        public MisoSoupCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(MisoSoup), "Miso Soups", 50, 0x284D)); // Assuming MisoSoup item ID is 0x1F8
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(RoninsSpiritKote), 1, "Tetsuya's Chef Kote")); // Assuming Chef Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Miso Soup Mastery quest!");
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

    public class GrandChefTetsuya : MondainQuester
    {
        [Constructable]
        public GrandChefTetsuya()
            : base("The Grand Chef", "Tetsuya")
        {
        }

        public GrandChefTetsuya(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FurBoots { Hue = Utility.Random(1, 3000), Name = "Tetsuya's Cooking Boots" });
            AddItem(new Cap { Hue = Utility.Random(1, 3000), Name = "Tetsuya's Chef Hat" });
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Tetsuya's Cooking Tunic" });
            AddItem(new HalfApron { Hue = Utility.Random(1, 3000), Name = "Tetsuya's Apron" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Tetsuya's Culinary Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MisoSoupCollectorQuest)
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
