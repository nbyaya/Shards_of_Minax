using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SpiderSilkCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Arachnid Enthusiast's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurous soul! I am Zorak, a devoted arachnid enthusiast with a peculiar request. " +
                       "I am currently researching the intricate web patterns of various spiders to create a magnificent tapestry. " +
                       "To complete this task, I need 50 SpiderSilk—an essential material for my project. " +
                       "If you could bring me these silks, I will reward you handsomely with gold, a unique Maxxia Scroll, and a special Spider Silk Chest that embodies the essence of my research.";
            }
        }

        public override object Refuse { get { return "Ah, I see. If you change your mind, please return. The tapestry needs your help."; } }

        public override object Uncomplete { get { return "I still require 50 SpiderSilk to complete my tapestry. Please bring them to me at your earliest convenience."; } }

        public override object Complete { get { return "You've done it! The SpiderSilk is just what I needed. With this, my research can advance significantly. Here is your reward, and thank you for your help!"; } }

        public SpiderSilkCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SpidersSilk), "Spider Silk", 50, 0xF8D)); // Adjust the item ID if necessary
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MinersPlateChest), 1, "Spider Silk Chest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Arachnid Enthusiast's Request!");
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

    public class SpiderSilkCollectorZorak : MondainQuester
    {
        [Constructable]
        public SpiderSilkCollectorZorak()
            : base("The Arachnid Enthusiast", "Spider Silk Collector Zorak")
        {
        }

        public SpiderSilkCollectorZorak(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2040; // Spiky hair style
            HairHue = 1152; // Hair hue (dark purple)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1152)); // Dark purple robe
            AddItem(new WizardsHat(1152)); // Matching dark purple wizard hat
            AddItem(new Sandals(0)); // Black sandals
            AddItem(new BlackStaff { Name = "Zorak's Arachnid Staff", Hue = 0x455 });
            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Bag of Arachnid Supplies";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SpiderSilkCollectorQuest)
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

    public class SpiderSilkRobe : Robe
    {
        [Constructable]
        public SpiderSilkRobe() : base(1152)
        {
            Name = "Spider Silk Robe";
            Hue = 1152; // Dark purple hue
        }

        public SpiderSilkRobe(Serial serial) : base(serial)
        {
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
