using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class LunaCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Luna's Herbal Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Luna, a dedicated herbalist. I am in need of a special ingredient to complete my latest potion—a potion that will grant extraordinary strength and resilience. " +
                       "The ingredient I seek is Nightshade, a rare and dangerous herb. I need 50 Nightshades to finish my potion. Your help would be immensely appreciated. " +
                       "In return for your efforts, I will reward you with gold, a unique Maxxia Scroll, and a special Enchanted Beret that I have woven with magical herbs. Will you assist me?";
            }
        }

        public override object Refuse { get { return "I understand. If you reconsider, please come back and assist me with the Nightshade collection."; } }

        public override object Uncomplete { get { return "You still need to bring me 50 Nightshades. Please gather all of them and return to me."; } }

        public override object Complete { get { return "You've brought the Nightshades! This will complete my potion and grant its power. Thank you so much for your help. Here are your rewards, as promised!"; } }

        public LunaCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Nightshade), "Nightshades", 50, 0xF88));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BeatniksBeret), 1, "Enchanted Beret"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed Luna's Herbal Collection quest!");
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

    public class LunaTheHerbalist : MondainQuester
    {
        [Constructable]
        public LunaTheHerbalist()
            : base("The Herbalist", "Luna the Herbalist")
        {
        }

        public LunaTheHerbalist(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2043; // Unique hair style
            HairHue = 0x21C; // Hair hue (bright pink)
        }

        public override void InitOutfit()
        {
            AddItem(new Kilt(1155)); // Purple skirt
            AddItem(new Cloak(1155)); // Purple robe
            AddItem(new Sandals(0x2D)); // White sandals
            AddItem(new FeatheredHat() { Hue = 0x115 }); // Flower headpiece for a magical touch
            Backpack backpack = new Backpack();
            backpack.Hue = 1155;
            backpack.Name = "Bag of Herbal Ingredients";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(LunaCollectorQuest)
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
