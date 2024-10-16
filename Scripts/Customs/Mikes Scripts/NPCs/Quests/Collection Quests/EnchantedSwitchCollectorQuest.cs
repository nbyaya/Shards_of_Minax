using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EnchantedSwitchCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Enchanted Switch Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Eldric the Wise. I need your help to collect 50 Enchanted Switches, " +
                       "which are crucial for my magical experiments. Your dedication in gathering these items will be rewarded " +
                       "with gold, a rare Maxxia Scroll, and a unique robe that will showcase your contribution to magical research.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, come back to me with the Enchanted Switches."; } }

        public override object Uncomplete { get { return "I still require 50 Enchanted Switches. Please return to me with them as soon as possible!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 Enchanted Switches I needed. Your help has been invaluable. " +
                       "Please accept these rewards as a token of my gratitude. Your dedication will not be forgotten!"; } }

        public EnchantedSwitchCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EnchantedSwitch), "Enchanted Switch", 50, 0x2F5C)); // Assuming Enchanted Switch item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BlacksmithsReinforcedGloves), 1, "Eldric's Magical Robe")); // Assuming Eldric's Magical Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Enchanted Switch Hunt quest!");
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

    public class EldricTheWise : MondainQuester
    {
        [Constructable]
        public EldricTheWise()
            : base("The Wise Mage", "Eldric")
        {
        }

        public EldricTheWise(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Eldric's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Eldric's Magic Hat" });
            AddItem(new QuarterStaff { Hue = Utility.Random(1, 3000), Name = "Eldric's Enchanted Staff" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Eldric's Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EnchantedSwitchCollectorQuest)
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
