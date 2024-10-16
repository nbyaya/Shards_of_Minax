using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class WoodenBowlOfStewCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Culinary Challenge"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Roderic, the Culinary Sage. The realm's taste buds are in peril! " +
                       "I seek 50 Wooden Bowls of Stew, each crafted with the finest ingredients from our lands. The stew is more than just food; " +
                       "it holds the essence of our history and culture. Your contribution will be honored with gold, a rare Maxxia Scroll, and " +
                       "a unique Culinary Sage's Outfit that will make you the talk of the town.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, come back with the Wooden Bowls of Stew."; } }

        public override object Uncomplete { get { return "I am still waiting for the 50 Wooden Bowls of Stew. Please, bring them to me to help preserve our culinary heritage!"; } }

        public override object Complete { get { return "Fantastic! You've brought the 50 Wooden Bowls of Stew! Your efforts are truly appreciated. Here are your rewards: gold, a rare Maxxia Scroll, and the Culinary Sage's Outfit. Wear it proudly!"; } }

        public WoodenBowlOfStewCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WoodenBowlOfStew), "Wooden Bowls of Stew", 50, 0x1604)); // Assuming Wooden Bowl of Stew item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(DreadlordsBoneChest), 1, "Culinary Sage's Outfit")); // Assuming Culinary Sage's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Culinary Challenge quest!");
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

    public class CulinarySageRoderic : MondainQuester
    {
        [Constructable]
        public CulinarySageRoderic()
            : base("The Culinary Sage", "Roderic")
        {
        }

        public CulinarySageRoderic(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Roderic's Culinary Tunic" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Roderic's Gourmet Hat" });
            AddItem(new BodySash { Hue = Utility.Random(1, 3000), Name = "Roderic's Special Recipe" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Roderic's Culinary Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WoodenBowlOfStewCollectorQuest)
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
