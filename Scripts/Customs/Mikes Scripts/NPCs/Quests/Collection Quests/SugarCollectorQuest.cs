using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SugarCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Sugar Craze"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave traveler! I am Baron Sweetstone, the master of confectioneries. My supplies of sugar have dwindled, and I need your aid to collect 50 SackOfSugar. " +
                       "These sacks are crucial for my secret recipes that enchant the sweets of our land. In return for your invaluable help, I shall reward you with gold, a rare Maxxia Scroll, " +
                       "and a delectable Sweetstone's Attire that will make you the envy of every confectioner.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the SackOfSugar."; } }

        public override object Uncomplete { get { return "I still need 50 SackOfSugar to continue my sweet creations. Please bring them to me!"; } }

        public override object Complete { get { return "Marvelous! You've brought me the 50 SackOfSugar I requested. Your assistance is most appreciated. Here are your rewards as a token of my gratitude. " +
                       "May your adventures be as sweet as the treats I create!"; } }

        public SugarCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SackOfSugar), "SackOfSugar", 50, 0x1039)); // Assuming SackOfSugar item ID is 0x1F6
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ResolutionKeepersSash), 1, "Sweetstone's Attire")); // Assuming Sweetstone's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Sugar Craze quest!");
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

    public class BaronSweetstone : MondainQuester
    {
        [Constructable]
        public BaronSweetstone()
            : base("The Master Confectioner", "Baron Sweetstone")
        {
        }

        public BaronSweetstone(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2044; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Baron's Confectionery Shirt" });
            AddItem(new ShortPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Baron's Sweet Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Baron's Golden Band" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Baron's Candy Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SugarCollectorQuest)
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
