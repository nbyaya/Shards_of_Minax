using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SausagePizzaCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Grand Pizza Feast"; } }

        public override object Description
        {
            get
            {
                return "Greetings, valiant hero! I am Pizzaro, the Master Chef of the grand feast! My great banquet is missing its pièce de résistance—" +
                       "50 Sausage Pizzas! These are no ordinary pizzas, but culinary masterpieces that have been enchanted to provide exceptional flavor. " +
                       "Assist me in gathering them, and in return, I shall reward you with gold, a rare Maxxia Scroll, and a fantastically flamboyant Chef's Attire.";
            }
        }

        public override object Refuse { get { return "I see. If you change your mind, I will be here, eager to welcome you back with open arms and empty plates."; } }

        public override object Uncomplete { get { return "The feast awaits! I still need 50 Sausage Pizzas. Bring them to me so we can begin the celebration!"; } }

        public override object Complete { get { return "Splendid work! You have gathered all 50 Sausage Pizzas. The feast will be unforgettable, thanks to you. " +
                       "Please accept these rewards as a token of my gratitude and wear this Chef's Attire with pride!"; } }

        public SausagePizzaCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SausagePizza), "Sausage Pizzas", 50, 0x1040)); // Assuming Sausage Pizza item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GreavesOfTheFallenStars), 1, "Chef's Attire")); // Assuming Chef's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Grand Pizza Feast quest!");
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

    public class MasterChefPizzaro : MondainQuester
    {
        [Constructable]
        public MasterChefPizzaro()
            : base("The Master Chef", "Pizzaro")
        {
        }

        public MasterChefPizzaro(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Pizzaro's Gourmet Shirt" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000), Name = "Pizzaro's Culinary Pants" });
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Pizzaro's Master Chef Apron" });
            AddItem(new Cap { Hue = Utility.Random(1, 3000), Name = "Pizzaro's Chef Hat" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Pizzaro's Recipe Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SausagePizzaCollectorQuest)
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
