using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class CheeseCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Great Cheese Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Casimir, the Grand Cheese Master of the Cheese Guild. My prized cheese collection has dwindled, " +
                       "and I need your help to restore it. Gather 50 pieces of Cheese for me, and I shall reward you handsomely with gold, a rare Maxxia Scroll, " +
                       "and the legendary Cheese Master’s Attire, renowned for its mystical charm and allure!";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Cheese."; } }

        public override object Uncomplete { get { return "I still require 50 pieces of Cheese. Please bring them to me so I can complete my collection!"; } }

        public override object Complete { get { return "Fantastic! You have collected the 50 pieces of Cheese I needed. Your dedication is most commendable. " +
                       "As a token of my appreciation, accept these rewards. May you savor the taste of victory on your travels!"; } }

        public CheeseCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CheeseWedge), "Cheese", 50, 0x97D)); // Assuming Cheese item ID is 0x9B2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(HealersFurCape), 1, "Cheese Master’s Cape")); // Assuming Cheese Master’s Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Great Cheese Hunt quest!");
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

    public class CheeseMasterCasimir : MondainQuester
    {
        [Constructable]
        public CheeseMasterCasimir()
            : base("The Cheese Master", "Casimir")
        {
        }

        public CheeseMasterCasimir(Serial serial)
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
            AddItem(new Doublet { Hue = Utility.Random(1, 3000), Name = "Casimir's Grand Doublet" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Casimir's Cheesy Hat" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Casimir's Golden Band" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Casimir's Cheese Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CheeseCollectorQuest)
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
