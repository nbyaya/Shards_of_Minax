using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BoardCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Grand Carpentry Challenge"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Elric, the master carpenter of this realm. My workshop is in dire need of " +
                       "an enormous quantity of boards for a grand construction project. I require 500 boards to complete the " +
                       "structure of a magnificent new guild hall. Will you aid me in gathering these boards? In return, I will " +
                       "reward you with gold, a rare Maxxia Scroll, and a unique Carpentry Arms that reflects the prestige of " +
                       "your accomplishment.";
            }
        }

        public override object Refuse { get { return "I understand if you're not interested. Should you reconsider, come back to me. The guild hall awaits!"; } }

        public override object Uncomplete { get { return "I still need 500 boards to finish the construction. Please bring them to me at once!"; } }

        public override object Complete { get { return "Wonderful! You've brought me all the boards I needed. The guild hall will be magnificent thanks to your help. " +
                       "Please accept these rewards as a token of my gratitude. Thank you!"; } }

        public BoardCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Board), "Boards", 500, 0x1BD7)); // Assuming Board item ID is 0x1BE
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(DisarmingLeatherArms), 1, "Carpentry Arms")); // Assuming Carpentry Hat is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Grand Carpentry Challenge quest!");
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

    public class BoardCollectorElric : MondainQuester
    {
        [Constructable]
        public BoardCollectorElric()
            : base("The Master Carpenter", "Elric the Board Collector")
        {
        }

        public BoardCollectorElric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Long hair style
            HairHue = 0x3F3; // Hair hue (light brown)
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(1150)); // Fancy shirt
            AddItem(new LongPants(1150)); // Long pants
            AddItem(new Shoes(1150)); // Matching shoes
            AddItem(new WideBrimHat { Name = "Elric's Carpentry Hat", Hue = 1150 }); // Custom Carpentry Hat
            AddItem(new Server.Items.FullApron { Name = "Elric's Apron", Hue = 1150 }); // Custom Apron
            AddItem(new Server.Items.WoodenShield { Name = "Elric's Shield", Hue = 1150 }); // Custom Wooden Shield
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Tool Bag of Elric";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BoardCollectorQuest)
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
