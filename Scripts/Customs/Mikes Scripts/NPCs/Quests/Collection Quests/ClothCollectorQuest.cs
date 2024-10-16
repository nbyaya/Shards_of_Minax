using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ClothCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Tailor's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, traveler! I am Enrico, a master tailor renowned for my exquisite garments. My workshop is currently " +
                       "in dire need of Bolts of Cloth to create a series of new outfits for a royal event. I need 50 Bolts of Cloth " +
                       "to fulfill this request. Will you assist me in gathering these materials? In return, you will be rewarded with " +
                       "gold, a rare Maxxia Scroll, and a unique Tailor's Ensemble that will make you stand out in any crowd.";
            }
        }

        public override object Refuse { get { return "Very well, if you change your mind, come back and see me. The royal event awaits!"; } }

        public override object Uncomplete { get { return "I still require 50 Bolts of Cloth to complete my request. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Excellent! You've brought me all the Bolts of Cloth I needed. The outfits will be splendid, thanks to your help. " +
                       "Please accept these rewards as a token of my appreciation. Thank you for your aid!"; } }

        public ClothCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BoltOfCloth), "Bolt of Cloth", 50, 0xF95)); // Assuming Bolt of Cloth item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(HerbalistsProtectiveHat), 1, "Tailor's Ensemble")); // Assuming Tailor's Ensemble is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Tailor's Request quest!");
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

    public class ClothCollectorEnrico : MondainQuester
    {
        [Constructable]
        public ClothCollectorEnrico()
            : base("The Tailor", "Cloth Collector Enrico")
        {
        }

        public ClothCollectorEnrico(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Tailor's hair style
            HairHue = 1150; // Hair hue (dark brown)
        }

        public override void InitOutfit()
        {
            AddItem(new Doublet(1150)); // Elegant doublet
            AddItem(new LongPants(1150)); // Long pants
            AddItem(new Shoes(1150)); // Matching shoes
            AddItem(new StrawHat { Name = "Enrico's Tailor Hat", Hue = 1150 }); // Custom Tailor Hat
            AddItem(new FullApron { Name = "Enrico's Apron", Hue = 1150 }); // Custom Apron
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Tailor's Supplies";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ClothCollectorQuest)
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

    // Custom item class for the Tailor's Ensemble
    public class TailorEnsemble : Item
    {
        [Constructable]
        public TailorEnsemble() : base(0x1C0B) // Custom item ID
        {
            Name = "Tailor's Ensemble";
            Hue = 1150; // Unique color
        }

        public TailorEnsemble(Serial serial) : base(serial)
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
