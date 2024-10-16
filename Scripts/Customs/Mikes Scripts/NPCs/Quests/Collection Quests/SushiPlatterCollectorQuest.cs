using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SushiPlatterCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Sushi Platter Pursuit"; } }

        public override object Description
        {
            get
            {
                return "Greetings, daring adventurer! I am Maki, the Sushi Master. My renowned sushi recipes have been disrupted by a lack of " +
                       "ingredients. I require 50 SushiPlatters to restore balance to my culinary creations. In return for your aid, I shall reward you " +
                       "with gold, a rare Maxxia Scroll, and a Sushi Master's Steps that will make you the envy of all sushi aficionados.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the SushiPlatters."; } }

        public override object Uncomplete { get { return "I still need those 50 SushiPlatters. Please bring them to me so I can continue my culinary masterpieces!"; } }

        public override object Complete { get { return "Splendid! You have gathered the 50 SushiPlatters I needed. Your contribution is greatly valued. Please accept these rewards " +
                       "as a token of my appreciation. May your future endeavors be as fulfilling as a perfectly crafted sushi roll!"; } }

        public SushiPlatterCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SushiPlatter), "SushiPlatters", 50, 0x2840)); // Assuming SushiPlatter item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SilentSteps), 1, "Sushi Master's Steps")); // Assuming Sushi Master's Kimono is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Sushi Platter Pursuit quest!");
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

    public class SushiMasterMaki : MondainQuester
    {
        [Constructable]
        public SushiMasterMaki()
            : base("The Sushi Master", "Maki")
        {
        }

        public SushiMasterMaki(Serial serial)
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
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Maki's Sushi Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new Bandana { Hue = Utility.Random(1, 3000), Name = "Maki's Sushi Bandana" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Maki's Culinary Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Maki's Sushi Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SushiPlatterCollectorQuest)
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
