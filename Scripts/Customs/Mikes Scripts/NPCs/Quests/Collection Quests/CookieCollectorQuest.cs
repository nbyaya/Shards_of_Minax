using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CookieCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Great Cookie Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Biscuitus, the Confectionary Master. My grand bakery has run out of the finest " +
                       "cookies, and I need your help to collect 50 of them. These cookies are no ordinary treats; they are enchanted with " +
                       "the essence of joy and laughter. For your aid, I will reward you with gold, a rare Maxxia Scroll, and the prestigious " +
                       "Confectioner's Attire, which will mark you as a true master of the culinary arts.";
            }
        }

        public override object Refuse { get { return "Very well, if you change your mind, come back with the cookies."; } }

        public override object Uncomplete { get { return "I still need 50 cookies to replenish my supplies. Please bring them to me!"; } }

        public override object Complete { get { return "Fantastic work! You have gathered the 50 cookies I needed. Your contribution is highly valued. Please accept these " +
                       "rewards as a token of my appreciation. May your travels be filled with sweetness and delight!"; } }

        public CookieCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Cookies), "Cookies", 50, 0x160b)); // Assuming Cookie item ID is 0x1234
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(FletchersPrecisionGloves), 1, "Confectioner's Attire")); // Assuming Confectioner's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Great Cookie Collection quest!");
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

    public class ConfectionaryMasterBiscuitus : MondainQuester
    {
        [Constructable]
        public ConfectionaryMasterBiscuitus()
            : base("The Confectionary Master", "Biscuitus")
        {
        }

        public ConfectionaryMasterBiscuitus(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Cap { Hue = Utility.Random(1, 3000), Name = "Biscuitus's Chef Hat" });
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Biscuitus's Apron" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Biscuitus's Baking Shirt" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Biscuitus's Baking Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CookieCollectorQuest)
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
