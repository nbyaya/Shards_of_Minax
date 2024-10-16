using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ValoriteOreCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Valorite Ore Pursuit"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave adventurer! I am Magnus, the Eternal Smith, and I require your aid. " +
                       "I seek to forge an artifact of unparalleled power, but I lack the crucial component: 50 Valorite Ores. " +
                       "These ores are known for their rarity and strength, and without them, my work cannot be completed. " +
                       "Your help will be rewarded with gold, a rare Maxxia Scroll, and a custom-crafted Valorite Smith's Armor.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Valorite Ores."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Valorite Ores. Bring them to me so I may continue my forging!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 Valorite Ores. My artifact is now complete, and your contribution is invaluable. " +
                       "Please accept these rewards as a token of my appreciation. May your adventures be as legendary as my creations!"; } }

        public ValoriteOreCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ValoriteOre), "Valorite Ores", 50, 0x19B9)); // Assuming Valorite Ore item ID is 0x19B9
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SpiritcallersGloves), 1, "Valorite Smith's Gloves")); // Assuming Valorite Smith's Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Valorite Ore Pursuit quest!");
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

    public class EternalSmithMagnus : MondainQuester
    {
        [Constructable]
        public EternalSmithMagnus()
            : base("The Eternal Smith", "Magnus")
        {
        }

        public EternalSmithMagnus(Serial serial)
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
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Magnus' Valorite Chestplate" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Magnus' Forging Helm" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Magnus' Forging Gauntlets" });
            AddItem(new BodySash { Hue = Utility.Random(1, 3000) });
            AddItem(new SmithHammer { Hue = Utility.Random(1, 3000), Name = "Magnus' Enchanted Smith Hammer" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Magnus' Forging Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ValoriteOreCollectorQuest)
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
