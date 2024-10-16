using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CitrineCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Gemologist's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Grelda, a skilled gemologist with a passion for precious stones. " +
                       "I am currently in need of 50 Citrine stones for a special project. These gems are vital to my research, " +
                       "and I can't proceed without them. Will you assist me in gathering these Citrines? In return, I will reward you " +
                       "with gold, a rare Maxxia Scroll, and a dazzling Gemologist's Cloak that will make you the envy of all gem enthusiasts.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind and decide to help, come back and see me. My project awaits!"; } }

        public override object Uncomplete { get { return "I still require 50 Citrine stones to proceed with my research. Please bring them to me at once!"; } }

        public override object Complete { get { return "Wonderful! You've brought me the Citrine stones I need. My research can now continue, and I am deeply grateful. " +
                       "Please accept these rewards as a token of my appreciation. Thank you for your assistance!"; } }

        public CitrineCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Citrine), "Citrine", 50, 0x3195)); // Assuming Citrine item ID is 0x1F6
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(HawkeyesGauntlets), 1, "Gemologist's Gauntlets")); // Assuming Gemologist's Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Gemologist's Request quest!");
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

    public class GemologistGrelda : MondainQuester
    {
        [Constructable]
        public GemologistGrelda()
            : base("The Gemologist", "Grelda")
        {
        }

        public GemologistGrelda(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Elegant hairstyle
            HairHue = 1152; // Hair hue (silver)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1157)); // Robe with gem hues
            AddItem(new Sandals(1157)); // Matching sandals
            AddItem(new Cloak { Name = "Gemologist's Cloak", Hue = 1157 }); // Custom Cloak
            AddItem(new Bag { Name = "Grelda's Gem Bag", Hue = 1157 }); // Custom Gem Bag
            AddItem(new QuarterStaff { Name = "Grelda's Gem Staff", Hue = 1157 }); // Custom Staff
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CitrineCollectorQuest)
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
