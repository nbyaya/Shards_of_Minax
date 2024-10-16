using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FurCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Fur Collector's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Brithor, the Master of the Wilds. I am in need of 50 pieces of Fur to complete my research " +
                       "on the creatures of the wilderness. These furs are vital for my studies and will greatly aid in my efforts. In exchange for your " +
                       "assistance, I will reward you with gold, a rare Maxxia Scroll, and a unique piece of my Wilds Armor.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Fur."; } }

        public override object Uncomplete { get { return "I still require 50 pieces of Fur. Please bring them to me to assist with my research!"; } }

        public override object Complete { get { return "Splendid work! You have collected the 50 pieces of Fur I needed. Your help is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your journeys in the wild be safe and prosperous!"; } }

        public FurCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Fur), "Fur", 50, 0x1875)); // Assuming Fur item ID is 0x1C6
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SkatersBaggyPants), 1, "Wilds Armor")); // Assuming Wilds Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Fur Collector's quest!");
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

    public class BrithorTheWildsMaster : MondainQuester
    {
        [Constructable]
        public BrithorTheWildsMaster()
            : base("The Master of the Wilds", "Brithor")
        {
        }

        public BrithorTheWildsMaster(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203D; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest { Hue = Utility.Random(1, 3000), Name = "Brithor's Wild Chestplate" });
            AddItem(new StuddedLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new Boots { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Brithor's Feathered Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Brithor's Wild Pack";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FurCollectorQuest)
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
