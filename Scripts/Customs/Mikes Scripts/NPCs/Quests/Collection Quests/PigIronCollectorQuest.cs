using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class PigIronCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Iron Forged Legacy"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave adventurer! I am Vark the Ironforged, a master blacksmith of the ancient clan of the Ironforge. " +
                       "Our once-glorious forges have gone cold due to the loss of essential Pig Iron. This iron is the lifeblood of our craft, " +
                       "and I need you to bring me 500 pieces of Pig Iron. In return, I will reward you with gold, a rare Maxxia Scroll, " +
                       "and a set of legendary Ironforged Shoes, crafted with the utmost care and enchantment. Help restore the fire to our forges!";
            }
        }

        public override object Refuse { get { return "I understand. Should you reconsider, return to me with the Pig Iron and we'll speak again."; } }

        public override object Uncomplete { get { return "I still require 500 Pig Iron. Bring them to me, and I will reward you handsomely!"; } }

        public override object Complete { get { return "Fantastic work! You have brought me the 500 Pig Iron. With this iron, our forges shall blaze once more! " +
                       "Accept these rewards as a token of my gratitude and admiration for your dedication."; } }

        public PigIronCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(PigIron), "Pig Iron", 500, 0xF8A)); // Assuming Pig Iron item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BobbySoxersShoes), 1, "Ironforged Shoes")); // Assuming Ironforged Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Iron Forged Legacy quest!");
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

    public class VarkTheIronforged : MondainQuester
    {
        [Constructable]
        public VarkTheIronforged()
            : base("The Iron Forged", "Vark")
        {
        }

        public VarkTheIronforged(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 50);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Vark's Ironforged Chestplate" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Vark's Ironforged Helm" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateGorget { Hue = Utility.Random(1, 3000) });
            AddItem(new OrderShield { Hue = Utility.Random(1, 3000), Name = "Vark's Reinforced Shield" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Vark's Crafting Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PigIronCollectorQuest)
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
