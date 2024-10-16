using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ShaftCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Shaft Collector's Challenge"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave adventurer! I am Galen the Craftmaster, renowned for my intricate creations. My latest masterpiece " +
                       "requires a vast amount of wooden shafts. I need 500 Shafts to complete it. These shafts are essential for the balance " +
                       "and precision of my work. If you aid me in this grand endeavor, I shall reward you with gold, a rare Maxxia Scroll, " +
                       "and a specially crafted Craftmaster's Outfit that is as unique as my creations.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Shafts."; } }

        public override object Uncomplete { get { return "I still require 500 Shafts. Please gather them for my grand creation!"; } }

        public override object Complete { get { return "Splendid! You have brought me the 500 Shafts I needed. Your contribution is invaluable. " +
                       "As a token of my gratitude, accept these rewards and wear the Craftmaster's Outfit with pride. May your adventures be prosperous!"; } }

        public ShaftCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Shaft), "Shafts", 500, 0x1BD4)); // Assuming Shaft item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(VestOfTheVeinSeeker), 1, "Craftmaster's Outfit")); // Assuming Craftmaster's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Shaft Collector's Challenge!");
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

    public class CraftmasterGalen : MondainQuester
    {
        [Constructable]
        public CraftmasterGalen()
            : base("The Craftmaster", "Galen")
        {
        }

        public CraftmasterGalen(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Galen's Craft Apron" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Galen's Crafting Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Spellbook { Hue = Utility.Random(1, 3000), Name = "Galen's Tome of Crafting" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Galen's Crafting Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ShaftCollectorQuest)
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
