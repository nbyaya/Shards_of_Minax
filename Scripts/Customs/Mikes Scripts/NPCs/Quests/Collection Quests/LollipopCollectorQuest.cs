using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class LollipopCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Sweet Tooth of Mirtha"; } }

        public override object Description
        {
            get
            {
                return "Ah, a fellow adventurer! I am Mirtha, the Candy Crafter. My sweet tooth has led me to crave the most delightful of treats—Lollipops! " +
                       "I'm in desperate need of 50 Lollipops to complete a grand confectionary creation. In exchange for your efforts, I will reward you with " +
                       "a bounty of gold, a rare Maxxia Scroll, and a dazzling outfit that will surely make you the talk of the town. Will you help me satisfy my craving?";
            }
        }

        public override object Refuse { get { return "Very well. If you reconsider, return to me with the Lollipops."; } }

        public override object Uncomplete { get { return "I still need 50 Lollipops. Please bring them to me so I can finish my grand creation!"; } }

        public override object Complete { get { return "Marvelous! You have collected the 50 Lollipops I needed. Your assistance is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be as sweet as these treats!"; } }

        public LollipopCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Lollipops), "Lollipops", 50, 0x468D)); // Assuming Lollipop item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WarriorsBelt), 1, "Candy Crafter's Outfit")); // Assuming Candy Crafter's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Sweet Tooth of Mirtha quest!");
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

    public class CandyCrafterMirtha : MondainQuester
    {
        [Constructable]
        public CandyCrafterMirtha()
            : base("The Candy Crafter", "Mirtha")
        {
        }

        public CandyCrafterMirtha(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Mirtha's Candy Tunic" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Mirtha's Lollipop Hat" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Mirtha's Candy Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(LollipopCollectorQuest)
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
