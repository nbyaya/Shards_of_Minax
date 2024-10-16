using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BaconCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Epicurean's Delight"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble adventurer! I am Roderick, a renowned epicurean and connoisseur of fine foods. " +
                       "My culinary creations are known far and wide, but currently, I face a dilemma. I require a large quantity of bacon " +
                       "for my next grand feast. I need 50 pieces of bacon to create a dish that will be remembered for ages. " +
                       "Will you help me gather this bacon? In return, I will reward you with gold, a rare Maxxia Scroll, and a unique " +
                       "Culinary Hat that is both stylish and practical.";
            }
        }

        public override object Refuse { get { return "I see you are not interested. Should you change your mind, come back and see me. My feast awaits!"; } }

        public override object Uncomplete { get { return "I still need 50 pieces of bacon to complete my feast. Please bring them to me at once!"; } }

        public override object Complete { get { return "Fantastic! You've brought me all the bacon I need. My feast will be a success thanks to you. " +
                       "Please accept these rewards as a token of my gratitude. Thank you for your help!"; } }

        public BaconCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Bacon), "Bacon", 50, 0x979)); // Assuming Bacon item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SeersLinkedSandals), 1, "Culinary Sandals")); // Assuming Culinary Hat is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Epicurean's Delight quest!");
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

    public class BaconCollectorRoderick : MondainQuester
    {
        [Constructable]
        public BaconCollectorRoderick()
            : base("The Epicurean", "Bacon Collector Roderick")
        {
        }

        public BaconCollectorRoderick(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Chef's hair style
            HairHue = 1161; // Hair hue (bright blonde)
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(1150)); // Fancy shirt
            AddItem(new ShortPants(1150)); // Short pants
            AddItem(new Shoes(1150)); // Matching shoes
            AddItem(new Cap { Name = "Roderick's Chef Hat", Hue = 1150 }); // Custom Chef Hat
            AddItem(new FullApron { Name = "Roderick's Apron", Hue = 1150 }); // Custom Apron
            AddItem(new Mace { Name = "Roderick's Rolling Pin", Hue = 1150 }); // Custom Rolling Pin
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bag of Culinary Ingredients";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BaconCollectorQuest)
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
