using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class PizzaCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Great Pizza Quest"; } }

        public override object Description
        {
            get
            {
                return "Greetings, daring adventurer! I am Orlo, the Great Pizza Enthusiast. My passion for pizza knows no bounds, " +
                       "but alas, I am in dire need of 50 pizzas to complete my grand feast. These pizzas will help me appease " +
                       "the legendary Pizza Spirits and ensure the prosperity of our town. In return for your generous help, I will " +
                       "reward you with gold, a rare Maxxia Scroll, and a unique Pizza Lover's Outfit that will make you the envy of " +
                       "all pizza aficionados!";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return with the pizzas I need."; } }

        public override object Uncomplete { get { return "I still need 50 pizzas to complete my feast. Please bring them to me!"; } }

        public override object Complete { get { return "Fantastic! You've gathered the 50 pizzas I needed. Your contribution is invaluable. " +
                       "Accept these rewards as a token of my gratitude, and may your love for pizza continue to flourish!"; } }

        public PizzaCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CheesePizza), "Pizzas", 50, 0x1040)); // Assuming Pizza item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SabreursJingasa), 1, "Pizza Lover's Outfit")); // Assuming Pizza Lover's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Great Pizza Quest!");
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

    public class PizzaLoversOutfit : Item
    {
        [Constructable]
        public PizzaLoversOutfit() : base(0x1C1) // Assuming the base item ID for the outfit is 0x1C1
        {
            Hue = Utility.Random(1, 3000);
            Name = "Pizza Lover's Outfit";
        }

        public PizzaLoversOutfit(Serial serial) : base(serial)
        {
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

    public class PizzaEnthusiastOrlo : MondainQuester
    {
        [Constructable]
        public PizzaEnthusiastOrlo()
            : base("The Great Pizza Enthusiast", "Orlo")
        {
        }

        public PizzaEnthusiastOrlo(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2046; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Doublet { Hue = Utility.Random(1, 3000), Name = "Orlo's Pizza Vest" });
            AddItem(new Shoes { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Orlo's Pizza Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldNecklace { Hue = Utility.Random(1, 3000), Name = "Orlo's Pizza Pendant" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Orlo's Pizza Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PizzaCollectorQuest)
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
