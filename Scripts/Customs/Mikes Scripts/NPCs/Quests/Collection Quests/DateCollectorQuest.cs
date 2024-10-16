using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DateCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Curious Case of the Dates"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Azara the Curious. I have recently discovered an ancient text that speaks of " +
                       "the mystical properties of Dates from the far-off deserts. To fully understand their significance, I require 50 " +
                       "Dates. These are not ordinary fruits; they hold ancient secrets that I must uncover. In return for your efforts, " +
                       "I will reward you with gold, a rare Maxxia Scroll, and an enchanting robe that embodies the very essence of the " +
                       "desert's allure.";
            }
        }

        public override object Refuse { get { return "If you change your mind, return to me with the Dates. Their secrets await to be uncovered!"; } }

        public override object Uncomplete { get { return "I still need 50 Dates. Please bring them to me so I can delve into their mysteries!"; } }

        public override object Complete { get { return "Fantastic work! You have brought me the 50 Dates I needed. Your assistance is invaluable. " +
                       "Accept these rewards as a token of my gratitude, and may the ancient secrets guide your path!"; } }

        public DateCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Dates), "Dates", 50, 0x1727)); // Assuming Date item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MarinersLuckyBoots), 1, "Desert Mystic's Boots")); // Assuming Desert Mystic's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Date Collector quest!");
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

    public class AzaraTheCurious : MondainQuester
    {
        [Constructable]
        public AzaraTheCurious()
            : base("Azara the Curious", "Azara")
        {
        }

        public AzaraTheCurious(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Azara's Enigmatic Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Azara's Desert Veil" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Azara's Ornate Bracelet" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Azara's Mysterious Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Azara's Secret Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DateCollectorQuest)
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
