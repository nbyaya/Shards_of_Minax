using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class VeriteOreCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Verite Treasure Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Draven, the Keeper of the Shimmering Forge. " +
                       "I have long searched for the fabled Verite Ore, which is said to possess the very essence of ancient magic. " +
                       "I need you to gather 50 pieces of Verite Ore for me. This precious metal will aid in crafting an artifact of immense power. " +
                       "In exchange for your effort, I will reward you with gold, a rare Maxxia Scroll, and a set of enchanted Bandana that gleams with the light of the forge.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Verite Ore."; } }

        public override object Uncomplete { get { return "I still require 50 Verite Ore. Please bring them to me so we can continue our work."; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 Verite Ore. With this, I can complete my grand design. " +
                       "Please accept these rewards as a token of my gratitude. May your path be illuminated by the fires of your own quests!"; } }

        public VeriteOreCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(VeriteOre), "Verite Ore", 50, 0x19B9)); // Assuming Verite Ore item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PopStarsSparklingBandana), 1, "Forged Bandana")); // Assuming VeriteForgeArmor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Verite Treasure Hunt quest!");
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

    public class DravenTheKeeper : MondainQuester
    {
        [Constructable]
        public DravenTheKeeper()
            : base("The Keeper of the Shimmering Forge", "Draven")
        {
        }

        public DravenTheKeeper(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new ChainCoif { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Draven's Shimmering Chestplate" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000), Name = "Draven's Gleaming Greaves" });
            AddItem(new PlateArms { Hue = Utility.Random(1, 3000), Name = "Draven's Shining Bracers" });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Draven's Forged Helm" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Draven's Enchanted Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(VeriteOreCollectorQuest)
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
