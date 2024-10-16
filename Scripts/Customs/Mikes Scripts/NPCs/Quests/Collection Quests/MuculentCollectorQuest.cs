using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MuculentCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Muculent Mystery"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave explorer! I am Taryn, the Muculent Connoisseur. Deep within the heart of the Whispering Forest lies a " +
                       "mysterious and potent fungus known as the Muculent. I seek to gather 50 of these rare specimens for my research on their " +
                       "unique properties. Your assistance in this endeavor will be rewarded with gold, a rare Maxxia Scroll, and an intricately " +
                       "enchanting Muculent Connoisseur's Attire. Will you help me unravel the secrets of the Muculent?"; 
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Muculent."; } }

        public override object Uncomplete { get { return "I still need 50 Muculent. Please bring them to me so I can continue my research!"; } }

        public override object Complete { get { return "Wonderful! You have gathered the 50 Muculent I needed. Your contribution to my research is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be ever prosperous!"; } }

        public MuculentCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Muculent), "Muculent", 50, 0x3188)); // Assuming Muculent item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(StarlightWozardsHat), 1, "Muculent Connoisseur's Hat")); // Assuming Muculent Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Muculent Mystery quest!");
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

    public class MuculentConnoisseurTaryn : MondainQuester
    {
        [Constructable]
        public MuculentConnoisseurTaryn()
            : base("The Muculent Connoisseur", "Taryn")
        {
        }

        public MuculentConnoisseurTaryn(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FemalePlateChest { Hue = Utility.Random(1, 3000), Name = "Taryn's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Taryn's Enchanted Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Taryn's Research Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Taryn's Unique Kilt" });
            AddItem(new GoldNecklace { Hue = Utility.Random(1, 3000), Name = "Taryn's Precious Amulet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Taryn's Research Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MuculentCollectorQuest)
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
