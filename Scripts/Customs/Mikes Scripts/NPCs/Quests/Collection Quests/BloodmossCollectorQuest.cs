using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BloodmossCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Botanist's Rare Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Yara, a devoted botanist in dire need of your assistance. " +
                       "I am conducting research on a powerful elixir that can enhance one's vitality, but I require a rare ingredient: Bloodmoss. " +
                       "I need 50 Bloodmoss to complete my elixir. Without them, my research will be at a standstill. " +
                       "Will you help me gather these rare materials? In return, I will reward you with gold, a unique Maxxia Scroll, and a Crimson Dagger, " +
                       "a symbol of my gratitude and a formidable weapon.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, please come back. My research depends on it."; } }

        public override object Uncomplete { get { return "I still need 50 Bloodmoss. Please bring them to me as soon as possible."; } }

        public override object Complete { get { return "You've gathered them all! I can't thank you enough. " +
                       "With these Bloodmoss, my research can continue. Here is your well-earned reward. Thank you!"; } }

        public BloodmossCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Bloodmoss), "Bloodmoss", 50, 0xF7B));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(JesterHatOfCommand), 1, "Crimson Jester Hat"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Botanist's Rare Request!");
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

    public class BloodmossCollectorYara : MondainQuester
    {

        [Constructable]
        public BloodmossCollectorYara()
            : base("The Botanist", "Crimson Collector Yara")
        {
        }

        public BloodmossCollectorYara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Random hair style
            HairHue = 1175; // Hair hue (bright red)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1175)); // Bright red robe
            AddItem(new WizardsHat(1175)); // Bright red wizard hat
            AddItem(new Sandals(33)); // Dark sandals
            AddItem(new GnarledStaff { Name = "Yara's Botanical Staff", Hue = 0 });
            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Bag of Botanical Supplies";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BloodmossCollectorQuest)
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
