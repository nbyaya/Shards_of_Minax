using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class ElvenFletchingCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Elven Archer's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Elaris, the Elven Archer. I require your assistance to gather 50 Elven Fletchings, " +
                       "which are crucial for my craft. In return for your effort, you will be rewarded with gold, a rare Maxxia Scroll, " +
                       "and a unique Elven Archer's Garb that symbolizes your dedication.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, bring me the fletchings."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Elven Fletchings. Please bring them to me when you can."; } }

        public override object Complete { get { return "Wonderful! You have brought me the 50 Elven Fletchings I needed. Your help is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your path be ever bright!"; } }

        public ElvenFletchingCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ElvenFletching), "Elven Fletching", 50, 0x5737)); // Assuming Elven Fletching item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(NaturesMuffler), 1, "Elven Archer's Attire")); // Assuming Elven Archer's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Elven Archer's Request quest!");
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

    public class ElvenArcherElaris : MondainQuester
    {
        [Constructable]
        public ElvenArcherElaris()
            : base("The Elven Archer", "Elaris")
        {
        }

        public ElvenArcherElaris(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherChest { Hue = Utility.Random(1, 3000), Name = "Elaris's Elven Armor" });
            AddItem(new LeatherLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherCap { Hue = Utility.Random(1, 3000), Name = "Elaris's Elven Hood" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherArms { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new Bow { Hue = Utility.Random(1, 3000), Name = "Elaris's Elven Bow" }); // Assuming Elven Bow is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Elaris's Adventurer's Pack";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ElvenFletchingCollectorQuest)
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
