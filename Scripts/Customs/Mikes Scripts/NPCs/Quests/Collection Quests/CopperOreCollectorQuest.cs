using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class CopperOreCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Copper Ore Collector"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave adventurer! I am Aric, the Keeper of the Hidden Forge. " +
                       "In the depths of our world, there lies a secret forge that harnesses the ancient powers of copper. " +
                       "To restore its lost strength, I need you to gather 50 Copper Ore. " +
                       "These ores are crucial to reignite the forge's magic. In return for your noble service, " +
                       "I shall grant you gold, a rare Maxxia Scroll, and the coveted Keeper's Vestments, woven with the essence of ancient copper.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Copper Ore."; } }

        public override object Uncomplete { get { return "I still require 50 Copper Ore. Please bring them to me so we can rekindle the forge's power!"; } }

        public override object Complete { get { return "Fantastic! You have collected the 50 Copper Ore I needed. The forge's power is now restored. " +
                       "Accept these rewards as a token of my gratitude. May your path be illuminated by the light of your deeds!"; } }

        public CopperOreCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CopperOre), "Copper Ore", 50, 0x19B7)); // Assuming Copper Ore item ID is 0x19B8
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MasterFletchersCoif), 1, "Keeper's Vestments")); // Assuming Keeper's Vestments is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Copper Ore Collector quest!");
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

    public class KeeperAric : MondainQuester
    {
        [Constructable]
        public KeeperAric()
            : base("The Keeper of the Hidden Forge", "Aric")
        {
        }

        public KeeperAric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2043; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Aric's Forged Helm" });
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Aric's Forged Chestplate" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Aric's Forged Gauntlets" });
            AddItem(new PlateArms { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Aric's Forge Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CopperOreCollectorQuest)
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
