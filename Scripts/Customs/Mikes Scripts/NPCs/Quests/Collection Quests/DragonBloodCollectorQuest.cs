using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DragonBloodCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Dragon's Bane"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Fyrn, the Dragon Slayer. I seek your assistance to gather 50 DragonBloods, " +
                       "a crucial ingredient for a powerful potion I am crafting. Your efforts will be rewarded with gold, a rare Maxxia Scroll, " +
                       "and a special Dragon Slayer's Garb that will make you stand out as a true hero.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the DragonBloods."; } }

        public override object Uncomplete { get { return "I still need 50 DragonBloods. Please bring them to me when you have collected them all!"; } }

        public override object Complete { get { return "Excellent! You have successfully gathered the 50 DragonBloods. Your help is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May you continue to be a beacon of bravery!"; } }

        public DragonBloodCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DragonBlood), "DragonBlood", 50, 0x4077)); // Assuming DragonBlood item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MinstrelsArmguards), 1, "Dragon Slayer's Garb")); // Assuming Dragon Slayer's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Dragon's Bane quest!");
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

    public class DragonSlayerFyrn : MondainQuester
    {
        [Constructable]
        public DragonSlayerFyrn()
            : base("The Dragon Slayer", "Fyrn")
        {
        }

        public DragonSlayerFyrn(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Fyrn's Dragon Armor" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Fyrn's Dragon Helm" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateArms { Hue = Utility.Random(1, 3000) });
            AddItem(new Boots { Hue = Utility.Random(1, 3000) });
            AddItem(new Longsword { Hue = Utility.Random(1, 3000), Name = "Fyrn's Dragon Slayer" }); // Assuming Dragon Slayer is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Fyrn's Adventurer's Pack";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DragonBloodCollectorQuest)
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
