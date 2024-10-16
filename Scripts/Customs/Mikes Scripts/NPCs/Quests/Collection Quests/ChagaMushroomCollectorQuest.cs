using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ChagaMushroomCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Mycologist's Bounty"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurous soul! I am Lyria, the esteemed mycologist and herbalist. " +
                       "My research into rare mushrooms has led me to a pressing need for Chaga Mushrooms. " +
                       "These mushrooms are crucial for my latest potion, and I need 50 of them to complete my experiment. " +
                       "If you can gather them for me, I will reward you with a generous amount of gold, a precious Maxxia Scroll, " +
                       "and a unique, enchanted hat that will make you stand out in any crowd.";
            }
        }

        public override object Refuse { get { return "Ah, I see you are not interested. If you change your mind, do come back and see me. " +
                                                   "The Chaga Mushrooms await!"; } }

        public override object Uncomplete { get { return "I still need 50 Chaga Mushrooms to complete my research. " +
                                                       "Please bring them to me as soon as you can!"; } }

        public override object Complete { get { return "Excellent work! You've brought me all the Chaga Mushrooms I needed. " +
                                                      "My potion will be a success, thanks to your help. Please accept these rewards " +
                                                      "as a token of my gratitude. Thank you, brave adventurer!"; } }

        public ChagaMushroomCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ChagaMushroom), "Chaga Mushroom", 50, 0x5743)); // Assuming Chaga Mushroom item ID is 0x1E2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SorcerersEnchantedLeggings), 1, "Lyria's Enchanted Leggings")); // Assuming Mage Hat is a suitable item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Mycologist's Bounty quest!");
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

    public class ChagaMushroomCollectorLyria : MondainQuester
    {
        [Constructable]
        public ChagaMushroomCollectorLyria()
            : base("The Mycologist", "Lyria the Mycologist")
        {
        }

        public ChagaMushroomCollectorLyria(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // A unique hair style
            HairHue = 1152; // Hair hue (deep red)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1152)); // Unique robe color
            AddItem(new Sandals(1152)); // Matching sandals
            AddItem(new StrawHat(1152) { Name = "Lyria's Enchanted Hat" }); // Custom Enchanted Hat
            AddItem(new GoldRing { Name = "Amulet of Fungi", Hue = 1152 }); // Custom Amulet
            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Lyria's Herbal Kit";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ChagaMushroomCollectorQuest)
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
