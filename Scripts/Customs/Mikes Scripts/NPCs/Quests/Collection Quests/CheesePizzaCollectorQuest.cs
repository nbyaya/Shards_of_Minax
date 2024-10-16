using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CheesePizzaCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Grand Cheese Pizza Quest"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave adventurer! I am Sir Brino the Cheesy, the grand master of all things pizza! " +
                       "I am on a quest to gather 50 Cheese Pizzas for the grand feast of the year. The cheese must be perfect, " +
                       "and the crust must be golden and crisp. Your help in this cheesy endeavor will earn you a bounty of gold, " +
                       "a rare Maxxia Scroll, and an exquisitely crafted Cheesy Crusader's Garb. Join me in this grand celebration!";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Cheese Pizzas."; } }

        public override object Uncomplete { get { return "I still require 50 Cheese Pizzas. The grand feast cannot begin without them!"; } }

        public override object Complete { get { return "Marvelous! You have collected the 50 Cheese Pizzas I required. The grand feast will be legendary! " +
                       "Please accept these rewards as a token of my gratitude. May your journey be as delightful as a slice of pizza!"; } }

        public CheesePizzaCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CheesePizza), "Cheese Pizzas", 50, 0x1040)); // Assuming Cheese Pizza item ID is 0xD1B
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ShogunsWisdomKabuto), 1, "Cheesy Crusader's Kabuto")); // Assuming Cheesy Crusader's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Grand Cheese Pizza Quest!");
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

    public class SirBrinoTheCheesy : MondainQuester
    {
        [Constructable]
        public SirBrinoTheCheesy()
            : base("Sir Brino the Cheesy", "Master of the Grand Feast")
        {
        }

        public SirBrinoTheCheesy(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Tunic { Hue = Utility.Random(1, 3000), Name = "Sir Brino's Cheesy Tunic" });
            AddItem(new Shoes { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Sir Brino's Cheesy Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Sir Brino's Golden Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Sir Brino's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CheesePizzaCollectorQuest)
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
