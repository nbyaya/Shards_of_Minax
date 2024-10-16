using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DarkSapphireCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Dark Sapphire Collector"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave traveler! I am Thalor, the Arcane Sage. Long ago, a powerful dark mage crafted an amulet " +
                       "imbued with the essence of 50 Dark Sapphires. These gems were lost to the ages, but their power remains " +
                       "undiminished. If you can recover 50 Dark Sapphires for me, I shall reward you with gold, a rare Maxxia Scroll, " +
                       "and the legendary Arcane Sage's Robe, an attire that holds ancient magical properties.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return with the Dark Sapphires."; } }

        public override object Uncomplete { get { return "I still require 50 Dark Sapphires. Please retrieve them so I may complete my research!"; } }

        public override object Complete { get { return "Remarkable! You have gathered the 50 Dark Sapphires I requested. Your bravery and resourcefulness are commendable. " +
                       "Please accept these rewards as a token of my gratitude. May the arcane energies guide your path!"; } }

        public DarkSapphireCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DarkSapphire), "Dark Sapphires", 50, 0x3192)); // Assuming Dark Sapphire item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WoodworkersInsightfulCap), 1, "Arcane Sage's Cap")); // Assuming Arcane Sage's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Dark Sapphire Collector quest!");
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

    public class ArcaneSageThalor : MondainQuester
    {
        [Constructable]
        public ArcaneSageThalor()
            : base("The Arcane Sage", "Thalor")
        {
        }

        public ArcaneSageThalor(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Thalor's Arcane Shirt" });
            AddItem(new ThighBoots { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Thalor's Enchanted Hat" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Thalor's Mystical Cloak" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Thalor's Arcane Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thalor's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DarkSapphireCollectorQuest)
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
