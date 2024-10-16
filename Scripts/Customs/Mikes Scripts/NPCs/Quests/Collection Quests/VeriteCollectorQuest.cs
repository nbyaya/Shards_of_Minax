using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class VeriteCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Alchemist's Precious Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Varen, a humble alchemist, and I find myself in a bit of a predicament. " +
                       "For years, I have been working on a grand experiment to create a potion of unparalleled power—one that could " +
                       "transform the very essence of reality itself. To complete this potion, I require a rare and precious material: Verite ingots. " +
                       "I need 50 Verite ingots to finish my research. They are crucial for the alchemical process, and without them, my experiment will be in vain. " +
                       "Will you help me gather these rare materials? In return, I will reward you with gold, a unique Maxxia Scroll, and Adventurers Boots, " +
                       "which is a token of my gratitude and a powerful tool in its own right.";
            }
        }

        public override object Refuse { get { return "Ah, I see. If you change your mind, come back to me. The fate of my experiment might depend on it."; } }

        public override object Uncomplete { get { return "The Verite ingots are still missing. I can't proceed without them. Please gather all 50 and return to me."; } }

        public override object Complete { get { return "You've done it! I can hardly believe it! The Verite ingots are exactly what I needed. " +
                       "With these, my experiment can proceed. Here is your well-earned reward. Thank you for your assistance!"; } }

        public VeriteCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(VeriteIngot), "Verite Ingots", 50, 0x1BF2));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(AdventurersBoots), 1, "Adventurers Boots"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Alchemist's Precious Request!");
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

    public class VeriteCollectorVaren : MondainQuester
    {

        [Constructable]
        public VeriteCollectorVaren()
            : base("The Alchemist", "Verite Collector Varen")
        {
        }

        public VeriteCollectorVaren(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Random hair style
            HairHue = 1153; // Hair hue (dark green)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1153)); // Green robe
            AddItem(new WizardsHat(1153)); // Green wizard hat
            AddItem(new Sandals(33)); // Dark sandals
            AddItem(new GnarledStaff { Name = "Varen's Alchemical Staff", Hue = 0 });
            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Bag of Alchemical Supplies";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(VeriteCollectorQuest)
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
