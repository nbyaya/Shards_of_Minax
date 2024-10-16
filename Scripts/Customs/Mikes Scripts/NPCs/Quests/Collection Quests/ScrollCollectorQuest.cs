using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ScrollCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Scribe's Challenge"; } }

        public override object Description
        {
            get
            {
                return "Greetings, traveler! I am Alistair, the ancient scribe of these lands. My study has been interrupted by a great " +
                       "need for Blank Scrolls. I require 500 Blank Scrolls to complete my tome of knowledge. In return, I shall reward " +
                       "you with a Maxxia Scroll and a special Scribe's Robe that will surely enhance your scholarly appearance.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, come back and see me. My study awaits your assistance!"; } }

        public override object Uncomplete { get { return "I still need 500 Blank Scrolls to complete my tome. Please bring them to me at once!"; } }

        public override object Complete { get { return "Excellent! You've gathered the Blank Scrolls I required. My tome will be complete thanks to you. " +
                       "Please accept these rewards as a token of my appreciation. Thank you for your help!"; } }

        public ScrollCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BlankScroll), "Blank Scroll", 500, 0xEF3)); // Assuming Blank Scroll item ID is 0x0E34
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MonksMeditativeRobe), 1, "Scribe's Robe")); // Assuming Scribe's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Scribe's Challenge quest!");
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

    public class ScrollCollectorAlistair : MondainQuester
    {
        [Constructable]
        public ScrollCollectorAlistair()
            : base("The Scribe", "Scroll Collector Alistair")
        {
        }

        public ScrollCollectorAlistair(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2041; // Scribe's hair style
            HairHue = 1152; // Hair hue (silver)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x0E6D)); // Elegant Robe
            AddItem(new Sandals(0x0E6D)); // Matching Sandals
            AddItem(new Cloak { Name = "Alistair's Cloak", Hue = 0x0E6D }); // Custom Cloak
            AddItem(new Spellbook { Name = "Ancient Spellbook", Hue = 0x0E6D }); // Custom Spellbook
            Backpack backpack = new Backpack();
            backpack.Hue = 0x0E6D;
            backpack.Name = "Scribe's Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ScrollCollectorQuest)
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
