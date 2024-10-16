using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MandrakeRootCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Enchanted Herb Gathering"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Liora, a skilled herbalist, and I find myself in need of your assistance. " +
                       "I am preparing a special potion that requires the essence of Mandrake Roots. " +
                       "I need 50 Mandrake Roots to complete this potion, which will grant me great power and insight. " +
                       "Would you be so kind as to gather them for me? In return, I will reward you with gold, a unique Maxxia Scroll, " +
                       "and an enchanted Dress as a token of my gratitude.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, please come back. My potion needs those Mandrake Roots."; } }

        public override object Uncomplete { get { return "The Mandrake Roots are still missing. Please bring me all 50 so that I can complete my potion."; } }

        public override object Complete { get { return "Wonderful! You've gathered all the Mandrake Roots I needed. With these, my potion will be complete. " +
                       "Here is your well-deserved reward. Thank you for your help!"; } }

        public MandrakeRootCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(MandrakeRoot), "Mandrake Roots", 50, 0xF86)); // MandrakeRoot item ID
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(StarletsFancyDress), 1, "Enchanted Dress")); // Replace with actual item if different
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Enchanted Herb Gathering quest!");
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

    public class MandrakeRootCollectorLiora : MondainQuester
    {
        [Constructable]
        public MandrakeRootCollectorLiora()
            : base("The Herbalist", "Mandrake Root Collector Liora")
        {
        }

        public MandrakeRootCollectorLiora(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Random hair style
            HairHue = 0xAAB; // Hair hue (light blue)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1157)); // Blue robe
            AddItem(new WizardsHat(1157)); // Blue wizard hat
            AddItem(new Sandals(0)); // Blue sandals
            AddItem(new QuarterStaff { Name = "Liora's Enchanted Staff", Hue = 0x4F });
            Backpack backpack = new Backpack();
            backpack.Hue = 1157;
            backpack.Name = "Bag of Herbal Ingredients";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MandrakeRootCollectorQuest)
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
