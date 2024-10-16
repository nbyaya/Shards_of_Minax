using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class AmethystCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Jewel of the Realm"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Arcturus, a renowned gemologist with a passion for rare and beautiful gemstones. " +
                       "I have recently come across a rare and ancient tome that requires the infusion of 50 Amethysts to complete its enchantment. " +
                       "Would you be willing to gather these Amethysts for me? In return, I shall reward you with a handsome sum of gold, a mystical Maxxia Scroll, " +
                       "and a truly unique Amethyst-Encrusted Chest that will make you the envy of all. Will you assist me?";
            }
        }

        public override object Refuse { get { return "I understand. Should you reconsider, I shall be here, waiting for your aid in this mystical endeavor."; } }

        public override object Uncomplete { get { return "I still need 50 Amethysts to complete the enchantment of the tome. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Splendid! You have brought me the Amethysts I needed. The enchantment will be completed with your help. " +
                       "As a token of my gratitude, please accept these rewards. Thank you for your invaluable assistance!"; } }

        public AmethystCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Amethyst), "Amethyst", 50, 0xF16)); // Assuming Amethyst item ID is 0x0F6C
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WarlordsChestguard), 1, "Amethyst-Encrusted Chest")); // Custom Amethyst-Encrusted Cloak
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Jewel of the Realm quest!");
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

    public class AmethystCollectorArcturus : MondainQuester
    {
        [Constructable]
        public AmethystCollectorArcturus()
            : base("The Gemologist", "Arcturus")
        {
        }

        public AmethystCollectorArcturus(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x204B; // Mystical hair style
            HairHue = 1152; // Hair hue (dark purple)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1152)); // Mystical robe
            AddItem(new Sandals(1152)); // Matching sandals
            AddItem(new PlateHelm { Name = "Arcturus's Crown", Hue = 1152 }); // Custom Crown
            AddItem(new QuarterStaff { Name = "Arcturus's Enchanted Staff", Hue = 1152 }); // Custom Enchanted Staff

            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Bag of Precious Gems";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(AmethystCollectorQuest)
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
