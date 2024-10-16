using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SulfurousAshCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Enchantress's Fiery Demand"; } }

        public override object Description
        {
            get
            {
                return "Greetings, valiant adventurer! I am Elara, an enchantress of the arcane arts. My latest spell requires " +
                       "a very rare component: Sulfurous Ash. I need 50 pieces of this volatile substance to complete a powerful enchantment. " +
                       "The ash will aid me in crafting a magical item of great significance. Will you assist me in gathering these crucial ingredients? " +
                       "In exchange, I shall reward you with gold, a rare Maxxia Scroll, and a mystical Bone of great power.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me, and we shall discuss this further."; } }

        public override object Uncomplete { get { return "I still need the Sulfurous Ash. Please gather all 50 pieces and return to me."; } }

        public override object Complete { get { return "You've done it! The Sulfurous Ash is exactly what I needed. With this, my enchantment can proceed. " +
                       "Here is your reward, and thank you for your valuable help!"; } }

        public SulfurousAshCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SulfurousAsh), "Sulfurous Ash", 50, 0xF8C));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GravekeepersBoneLegs), 1, "Mystical Enchanting Bone"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Enchantress's Fiery Demand!");
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

    public class SulfurousAshCollectorElara : MondainQuester
    {

        [Constructable]
        public SulfurousAshCollectorElara()
            : base("The Enchantress", "Sulfurous Ash Collector Elara")
        {
        }

        public SulfurousAshCollectorElara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Random hair style
            HairHue = 1152; // Hair hue (light blue)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1152)); // Light blue robe
            AddItem(new StrawHat(1152)); // Light blue wizard hat
            AddItem(new Sandals(33)); // Dark sandals
            AddItem(new BlackStaff { Name = "Elara's Enchanting Staff", Hue = 1152, LootType = LootType.Blessed });
            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Bag of Enchanting Ingredients";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SulfurousAshCollectorQuest)
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
