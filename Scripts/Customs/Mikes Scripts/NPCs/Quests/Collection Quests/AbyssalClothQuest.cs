using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class AbyssalClothQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Abyssal Fabrication"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave soul! I am Elara, the Arcane Weaver. My craft relies on rare and mystical materials, and " +
                       "currently, I need a substantial amount of Abyssal Cloth to complete my latest enchantment. I require 50 pieces " +
                       "of Abyssal Cloth to continue my work. Will you assist me in gathering them? In gratitude, I will reward you with " +
                       "a sum of gold, a coveted Maxxia Scroll, and a unique and mystical Veil that will mark your deeds.";
            }
        }

        public override object Refuse { get { return "Very well, if you change your mind and wish to assist me, return and we shall speak again."; } }

        public override object Uncomplete { get { return "I am still in need of 50 pieces of Abyssal Cloth. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "You've brought me all the Abyssal Cloth I need! Your assistance is invaluable. Accept these rewards as a token of my appreciation."; } }

        public AbyssalClothQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(AbyssalCloth), "Abyssal Cloth", 50, 0x1767)); // Assuming Abyssal Cloth item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GazeCapturingVeil), 1, "Mystic Veil")); // Assuming Mystic Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Abyssal Fabrication quest!");
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

    public class AbyssalClothWeaver : MondainQuester
    {
        [Constructable]
        public AbyssalClothWeaver()
            : base("The Arcane Weaver", "Elara")
        {
        }

        public AbyssalClothWeaver(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2040; // Mystical hair style
            HairHue = 1153; // Hair hue (dark purple)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1152)); // Unique robe
            AddItem(new Sandals(1152)); // Matching sandals
            AddItem(new Cloak { Hue = 1152, Name = "Elara's Enchanted Cloak" }); // Custom enchanted cloak
            AddItem(new BlackStaff { Hue = 1152, Name = "Elara's Mystical Staff" }); // Custom mystical staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Bag of Arcane Fabrics";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(AbyssalClothQuest)
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
