using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class AnimalPheromoneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Beastly Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Thalion, a renowned beast master and naturalist. " +
                       "I am currently working on a special potion that requires the essence of various creatures. " +
                       "To complete this potion, I need 50 Animal Pheromones. Can you help me gather these pheromones? " +
                       "In return, I will reward you with gold, a rare Maxxia Scroll, and a unique Beast Master's Tunic that is " +
                       "both stylish and practical.";
            }
        }

        public override object Refuse { get { return "I see you are not interested. Should you change your mind, come back and see me. My potion awaits!"; } }

        public override object Uncomplete { get { return "I still need 50 Animal Pheromones to complete my potion. Please bring them to me!"; } }

        public override object Complete { get { return "Excellent! You've brought me all the Animal Pheromones I need. The potion will be complete thanks to you. " +
                       "Please accept these rewards as a token of my gratitude. Thank you for your help!"; } }

        public AnimalPheromoneQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(AnimalPheromone), "Animal Pheromone", 50, 0x182F)); // Assuming AnimalPheromone item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GlamRockersJacket), 1, "Beast Master's Tunic")); // Assuming Beast Master's Tunic is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Beastly Request quest!");
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

    public class BeastMasterThalion : MondainQuester
    {
        [Constructable]
        public BeastMasterThalion()
            : base("The Beast Master", "Thalion")
        {
        }

        public BeastMasterThalion(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Long hair style
            HairHue = 0x2A; // Hair hue (dark brown)
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(1151)); // Unique fancy shirt
            AddItem(new LongPants(1151)); // Matching long pants
            AddItem(new Boots(1151)); // Matching boots
            AddItem(new Cloak { Name = "Thalion's Cloak", Hue = 1152 }); // Custom cloak
            AddItem(new GoldRing { Name = "Thalion's Beast Master Talisman", Hue = 1152 }); // Custom talisman
            AddItem(new QuarterStaff { Name = "Thalion's Staff of Beasts", Hue = 1152 }); // Custom staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Bag of Beastly Essences";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(AnimalPheromoneQuest)
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
