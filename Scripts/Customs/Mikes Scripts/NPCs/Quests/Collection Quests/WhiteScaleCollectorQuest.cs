using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class WhiteScaleCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The White Scale Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Draconis, the Ancient Dragon Tamer. Long ago, I bound my soul to the great " +
                       "dragons of the land, and their scales hold the key to maintaining the ancient bond we share. I require 50 White Scales " +
                       "to strengthen my magic and ensure that our bond remains unbroken. In return for your assistance, you shall be rewarded " +
                       "with gold, a rare Maxxia Scroll, and the prestigious Draconis' Dragonkeeper's Garb, a testament to your bravery.";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, return to me with the White Scales."; } }

        public override object Uncomplete { get { return "I still need 50 White Scales. Return to me once you have gathered them all."; } }

        public override object Complete { get { return "Ah, you have done it! I am most grateful for your efforts. Here are your rewards: gold, a Maxxia Scroll, and the " +
                       "Draconis' Dragonkeeper's Garb. Wear it with pride and honor the bond we share with the dragons."; } }

        public WhiteScaleCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WhiteScales), "White Scales", 50, 0x26B4)); // Assuming White Scale item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(TechGurusGlasses), 1, "Draconis' Dragonkeeper's Garb")); // Assuming Dragonkeeper's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the White Scale Collector quest!");
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

    public class Draconis : MondainQuester
    {
        [Constructable]
        public Draconis()
            : base("The Ancient Dragon Tamer", "Draconis")
        {
        }

        public Draconis(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Draconis' Dragonkeeper Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new NorseHelm { Hue = Utility.Random(1, 3000), Name = "Draconis' Dragonkeeper Helm" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Draconis' Dragonkeeper Gloves" });
            AddItem(new PlateGorget { Hue = Utility.Random(1, 3000), Name = "Draconis' Dragonkeeper Gorget" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Draconis' Dragonkeeper Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WhiteScaleCollectorQuest)
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
