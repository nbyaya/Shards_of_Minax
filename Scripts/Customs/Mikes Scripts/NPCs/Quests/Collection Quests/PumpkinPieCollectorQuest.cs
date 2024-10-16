using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class PumpkinPieCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Grand Pumpkin Pie Gathering"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave wanderer! I am Baron Berrington, the Confectioner of the Harvest. I have a special task for you. " +
                       "It seems that my secret recipe for Pumpkin Pie has been stolen and scattered across the land. To restore the festive spirit " +
                       "of the Harvest Festival, I need you to collect 50 Pumpkin Pies. In exchange for your help, I will reward you with gold, " +
                       "a rare Maxxia Scroll, and a splendid Pumpkin Confectioner’s Attire that will surely make you the talk of the town!";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return with the Pumpkin Pies, and I'll be here to assist."; } }

        public override object Uncomplete { get { return "I still need 50 Pumpkin Pies to complete the festival preparations. Please bring them to me!"; } }

        public override object Complete { get { return "Ah, marvelous! You've gathered all 50 Pumpkin Pies. My gratitude knows no bounds. Please accept these rewards as a token of my appreciation. " +
                       "May your adventures be as sweet as these pies!"; } }

        public PumpkinPieCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(PumpkinPie), "Pumpkin Pies", 50, 0x1041)); // Assuming Pumpkin Pie item ID is 0x1039
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(AssassinsMaskedCap), 1, "Pumpkin Confectioner’s Attire")); // Assuming Pumpkin Confectioner’s Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Grand Pumpkin Pie Gathering quest!");
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

    public class PumpkinConfectionerBaron : MondainQuester
    {
        [Constructable]
        public PumpkinConfectionerBaron()
            : base("The Pumpkin Confectioner", "Baron Berrington")
        {
        }

        public PumpkinConfectionerBaron(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204A; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Baron's Harvest Tunic" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Baron’s Festive Hat" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new HalfApron { Hue = Utility.Random(1, 3000), Name = "Baron's Chef Apron" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Baron's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PumpkinPieCollectorQuest)
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
