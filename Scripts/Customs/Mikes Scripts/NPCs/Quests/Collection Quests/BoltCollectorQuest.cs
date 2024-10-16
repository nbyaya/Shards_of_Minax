using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BoltCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Great Bolt Bonanza"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Gearis, the master inventor. I am currently working on a grand project, " +
                       "but I need a vast amount of bolts to complete my latest invention. Specifically, I need 500 bolts. " +
                       "Will you assist me in gathering these bolts? In return, I will reward you with gold, a rare Maxxia Scroll, " +
                       "and a special Inventor's Arms that is both practical and fashionable.";
            }
        }

        public override object Refuse { get { return "Ah, I see. If you change your mind, come back and see me. My invention needs those bolts!"; } }

        public override object Uncomplete { get { return "I still need 500 bolts to finish my invention. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Excellent work! You've brought me all the bolts I needed. My invention will be complete thanks to you. " +
                       "Please accept these rewards as a token of my appreciation. Thank you for your help!"; } }

        public BoltCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Bolt), "Bolt", 500, 0x1BFB)); // Assuming Bolt item ID is 0x1C4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ProspectorsArms), 1, "Inventor's Arms")); // Assuming Inventor's Hat is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Great Bolt Bonanza quest!");
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

    public class BoltCollectorGearis : MondainQuester
    {
        [Constructable]
        public BoltCollectorGearis()
            : base("The Master Inventor", "Bolt Collector Gearis")
        {
        }

        public BoltCollectorGearis(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203D; // Inventor's hair style
            HairHue = 1150; // Hair hue (bright blonde)
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(1150)); // Fancy shirt
            AddItem(new ShortPants(1150)); // Short pants
            AddItem(new Shoes(1150)); // Matching shoes
            AddItem(new StrawHat { Name = "Gearis' Inventor Hat", Hue = 1150 }); // Custom Inventor's Hat
            AddItem(new FullApron { Name = "Gearis' Work Apron", Hue = 1150 }); // Custom Work Apron
            AddItem(new Hammer { Name = "Gearis' Wrench", Hue = 1150 }); // Custom Wrench
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bag of Engineering Supplies";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BoltCollectorQuest)
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
