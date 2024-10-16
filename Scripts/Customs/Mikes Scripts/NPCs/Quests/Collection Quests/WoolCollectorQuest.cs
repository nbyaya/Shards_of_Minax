using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class WoolCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Weaver's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble adventurer! I am Aric, the Weaver of Myths. The threads of fate have unraveled, " +
                       "and I am in dire need of 50 Wool to restore the enchanted tapestries that protect our realm. " +
                       "These woolen threads are integral to my magic, and only your valiant efforts can save them. " +
                       "In exchange for your assistance, I will bestow upon you a reward of gold, a rare Maxxia Scroll, " +
                       "and a distinctive Weaver's Tunic, woven with the very essence of our world.";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, return to me with the Wool."; } }

        public override object Uncomplete { get { return "I still require 50 Wool. Please gather them for me so that I may restore the magical tapestries."; } }

        public override object Complete { get { return "Marvelous! You have brought me the 50 Wool I needed. Your bravery and commitment have saved our realm. " +
                       "Please accept these rewards as a token of my profound gratitude. May your journey be filled with wonder and adventure!"; } }

        public WoolCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Wool), "Wool", 50, 0xDF8)); // Assuming Wool item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BeastmastersTunic), 1, "Weaver's Tunic")); // Assuming Weaver's Tunic is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed The Weaver's Request quest!");
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

    public class WeaverAric : MondainQuester
    {
        [Constructable]
        public WeaverAric()
            : base("The Weaver of Myths", "Aric")
        {
        }

        public WeaverAric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Aric's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Aric's Mystical Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Aric's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WoolCollectorQuest)
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
