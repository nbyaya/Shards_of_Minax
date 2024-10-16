using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EssenceOrderCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Essence Order's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Elara, the Essence Keeper. I need your assistance to collect 50 Essence Orders, " +
                       "which are crucial for my research into the ancient magics. In return for your efforts, I will reward you with gold, " +
                       "a rare Maxxia Scroll, and a special Essence Keeper's Robe that will set you apart from others.";
            }
        }

        public override object Refuse { get { return "I see. If you change your mind, come back and bring me the Essence Orders."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Essence Orders. Please bring them to me so that I may continue my research!"; } }

        public override object Complete { get { return "Fantastic! You have gathered the 50 Essence Orders I requested. Your help has been invaluable. " +
                       "Please accept these rewards as a sign of my gratitude. May your path be illuminated with knowledge!"; } }

        public EssenceOrderCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EssenceOrder), "Essence Order", 50, 0x571C)); // Assuming Essence Order item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(CouturiersSundress), 1, "Essence Keeper's Robe")); // Assuming Essence Keeper's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Essence Order's Request quest!");
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

    public class EssenceKeeperElara : MondainQuester
    {
        [Constructable]
        public EssenceKeeperElara()
            : base("The Essence Keeper", "Elara")
        {
        }

        public EssenceKeeperElara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Elara's Essence Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new ClothNinjaHood { Hue = Utility.Random(1, 3000) });
            AddItem(new QuarterStaff { Hue = Utility.Random(1, 3000), Name = "Elara's Essence Staff" }); // Assuming Essence Staff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Elara's Scholar's Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EssenceOrderCollectorQuest)
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
