using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GarlicCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Herbalist's Vital Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble adventurer! I am Erina, a dedicated herbalist, and I am in desperate need of your assistance. " +
                       "I am working on a remedy to cure a deadly plague that is spreading across the land. " +
                       "To complete this vital remedy, I require a significant amount of garlic. " +
                       "I need 50 Garlic bulbs to prepare the potion. Without them, I cannot create the cure that our people desperately need. " +
                       "Will you help me gather these essential ingredients? In return, I will reward you with gold, a unique Maxxia Scroll, " +
                       "and a Garlic Muffler, a token of my appreciation and a handy tool for any adventurer.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, please return. Many lives depend on this remedy."; } }

        public override object Uncomplete { get { return "I still need more Garlic bulbs. Please bring all 50 so I can prepare the remedy."; } }

        public override object Complete { get { return "You've done it! These Garlic bulbs are exactly what I needed. " +
                       "Now I can proceed with creating the cure. Here is your well-earned reward. Thank you for your heroic efforts!"; } }

        public GarlicCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Garlic), "Garlic", 50, 0xF84));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BaristasMuffler), 1, "Garlic Muffler"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Herbalist's Vital Request!");
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

    public class GarlicCollectorErina : MondainQuester
    {

        [Constructable]
        public GarlicCollectorErina()
            : base("The Herbalist", "Garlic Collector Erina")
        {
        }

        public GarlicCollectorErina(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Random hair style
            HairHue = 1150; // Hair hue (light blue)
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress(1150)); // Light blue fancy dress
            AddItem(new Sandals(1153)); // Dark green sandals
            AddItem(new FeatheredHat(1150)); // Light blue feathered hat
            AddItem(new QuarterStaff { Name = "Erina's Herbal Staff", Hue = 0 });
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bag of Herbal Supplies";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GarlicCollectorQuest)
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
