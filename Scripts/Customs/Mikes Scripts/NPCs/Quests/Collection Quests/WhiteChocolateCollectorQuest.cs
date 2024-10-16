using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class WhiteChocolateCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Sweet Tooth of Elara"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings, brave adventurer! I am Elara, the Culinary Enchantress. I am in desperate need of 50 White Chocolates. " +
                       "You see, these delectable treats hold a special place in my heart and are essential for my magical culinary experiments. " +
                       "In exchange for your help, I shall reward you with gold, a rare Maxxia Scroll, and a unique Culinary Enchantress's Gloves. " +
                       "Embark on this sweet quest and bring joy to both your taste buds and my magical endeavors!";
            }
        }

        public override object Refuse { get { return "Very well. If you decide to help later, return to me with the White Chocolates."; } }

        public override object Uncomplete { get { return "I still require 50 White Chocolates. Please bring them to me so I can continue my experiments!"; } }

        public override object Complete { get { return "Fantastic work! You have gathered the 50 White Chocolates I needed. Your help has been invaluable. " +
                       "As a token of my gratitude, please accept these rewards. May your adventures be as sweet as the chocolates you brought!"; } }

        public WhiteChocolateCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WhiteChocolate), "White Chocolates", 50, 0xF11)); // Assuming White Chocolate item ID is 0x0F3E
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GlovesOfCommand), 1, "Culinary Enchantress's Gloves")); // Assuming Culinary Enchantress's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Sweet Tooth of Elara quest!");
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

    public class CulinaryEnchantressElara : MondainQuester
    {
        [Constructable]
        public CulinaryEnchantressElara()
            : base("The Culinary Enchantress", "Elara")
        {
        }

        public CulinaryEnchantressElara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress { Hue = Utility.Random(1, 3000), Name = "Elara's Enchanted Dress" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Elara's Culinary Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Necklace { Hue = Utility.Random(1, 3000), Name = "Elara's Enchanted Necklace" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Elara's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WhiteChocolateCollectorQuest)
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
