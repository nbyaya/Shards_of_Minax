using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class ZoogiFungusCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Enigmatic Zoogi Quest"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave explorer! I am Morgath, the Keeper of the Forbidden Grove. " +
                       "Legends tell of an ancient ritual that requires the essence of 50 ZoogiFungus. " +
                       "These fungi are said to contain the secrets of an ancient forest's magic. " +
                       "Your aid in gathering these will be rewarded handsomely. In return, I will grant you gold, a rare Maxxia Scroll, " +
                       "and a Buckler imbued with the forest's essence, unlike anything you have seen.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the ZoogiFungus."; } }

        public override object Uncomplete { get { return "I still require 50 ZoogiFungus. Please gather them to aid in my ritual."; } }

        public override object Complete { get { return "Marvelous! You have collected the 50 ZoogiFungus I needed. Your bravery will be remembered. " +
                       "As a token of gratitude, accept these rewards. May the magic of the forest guide you!"; } }

        public ZoogiFungusCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ZoogiFungus), "ZoogiFungus", 50, 0x26B7)); // Assuming ZoogiFungus item ID is 0xF8F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(DragonscaleBuckler), 1, "Enchanted Forest Buckler")); // Assuming Enchanted Forest Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Enigmatic Zoogi Quest!");
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

    public class KeeperOfTheForbiddenGrove : MondainQuester
    {
        [Constructable]
        public KeeperOfTheForbiddenGrove()
            : base("The Keeper of the Forbidden Grove", "Morgath")
        {
        }

        public KeeperOfTheForbiddenGrove(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherChest { Hue = Utility.Random(1, 3000), Name = "Morgath's Forest Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new StrawHat { Hue = Utility.Random(1, 3000), Name = "Morgath's Enchanted Leaf Hat" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Morgath's Mystical Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Morgath's Forest Kilt" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Morgath's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ZoogiFungusCollectorQuest)
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
