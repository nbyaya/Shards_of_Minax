using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DarkChocolateCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Dark Chocolate Connoisseur"; } }

        public override object Description
        {
            get
            {
                return "Greetings, bold adventurer! I am Melisandre, the Chocolate Alchemist. Long ago, I crafted a powerful potion " +
                       "that requires the essence of 50 Dark Chocolates to perfect. These rare treats are essential for completing the " +
                       "potion, and only someone of your skill can assist me in this quest. In gratitude, I will reward you with gold, " +
                       "a Maxxia Scroll, and a truly unique confectioner’s outfit. Help me complete this magical concoction!";
            }
        }

        public override object Refuse { get { return "Very well. If you reconsider, return to me with the Dark Chocolates."; } }

        public override object Uncomplete { get { return "I still need 50 Dark Chocolates to finish my potion. Please bring them to me!"; } }

        public override object Complete { get { return "Wonderful! You have collected the 50 Dark Chocolates I needed. Your assistance has been invaluable. " +
                       "Please accept these rewards as a token of my appreciation. May your future adventures be filled with sweetness!"; } }

        public DarkChocolateCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DarkChocolate), "Dark Chocolates", 50, 0xF10)); // Assuming Dark Chocolate item ID is 0x1F6
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(AnglersSeabreezeCloak), 1, "Confectioner's Outfit")); // Assuming Confectioner's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Dark Chocolate Connoisseur quest!");
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

    public class ConfectionerMelisandre : MondainQuester
    {
        [Constructable]
        public ConfectionerMelisandre()
            : base("The Chocolate Alchemist", "Melisandre")
        {
        }

        public ConfectionerMelisandre(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2043; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherChest { Hue = Utility.Random(1, 3000), Name = "Melisandre's Chocolate Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Melisandre's Confectioner's Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Crossbow { Hue = Utility.Random(1, 3000), Name = "Melisandre's Alchemical Wand" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Melisandre's Enchanted Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DarkChocolateCollectorQuest)
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
