using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GinsengCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Herbalist's Essential Gathering"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble adventurer! I am Arwen, a dedicated herbalist, and I am in desperate need of a particular herb " +
                       "for my latest potion—a potion that can bring balance to the forces of nature. I need 50 Ginseng roots to complete " +
                       "this potion. Ginseng is known for its restorative properties and is essential for my work. Will you aid me in this task? " +
                       "As a token of my appreciation, I will reward you with gold, a valuable Maxxia Scroll, and a special herbalist's Crown.";
            }
        }

        public override object Refuse { get { return "I understand. If you reconsider, please return and help me with my herbalist’s task."; } }

        public override object Uncomplete { get { return "The Ginseng roots are still missing. Please bring me all 50 so that I may continue my work."; } }

        public override object Complete { get { return "You've done it! The Ginseng roots are just what I needed. Thank you for your help! Here is your reward—gold, a Maxxia Scroll, and a special staff."; } }

        public GinsengCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Ginseng), "Ginseng Roots", 50, 0xF85));
            AddReward(new BaseReward(typeof(Gold), 3000, "3000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PumpkinKingsCrown), 1, "Herbalist's Crown"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Herbalist's Essential Gathering quest!");
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

    public class HerbalistArwen : MondainQuester
    {
        [Constructable]
        public HerbalistArwen()
            : base("The Herbalist", "Arwen the Herbalist")
        {
        }

        public HerbalistArwen(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Long hair
            HairHue = 0x83F; // Dark brown
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x1F8)); // Forest green robe
            AddItem(new WizardsHat(0x1F8)); // Matching hat
            AddItem(new Sandals(0x1F8)); // Matching sandals
            AddItem(new GnarledStaff { Name = "Arwen's Herbalist Staff", Hue = 0x1F8 });
            Backpack backpack = new Backpack();
            backpack.Hue = 0x1F8;
            backpack.Name = "Bag of Herbal Supplies";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GinsengCollectorQuest)
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
