using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GoblinBloodCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Goblin Blood Harvest"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Malorn, the Alchemist of Shadows. " +
                       "For centuries, I have been studying the dark alchemical properties of Goblin Blood. " +
                       "I require 500 Goblin Blood to complete my research. Your aid in this matter will be rewarded " +
                       "with gold, a rare Maxxia Scroll, and a unique Alchemist's Cloak adorned with mystical symbols.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Goblin Blood."; } }

        public override object Uncomplete { get { return "I still require 500 Goblin Blood. Collect more and bring them to me to continue my research!"; } }

        public override object Complete { get { return "Outstanding! You have collected the 500 Goblin Blood I needed. Your help is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your path be filled with fortune and mystery!"; } }

        public GoblinBloodCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(GoblinBlood), "Goblin Blood", 500, 0x572C)); // Assuming Goblin Blood item ID is 0xF6B
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SilentNightCloak), 1, "Alchemist's Cloak")); // Assuming Alchemist's Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Goblin Blood Harvest quest!");
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

    public class AlchemistMalorn : MondainQuester
    {
        [Constructable]
        public AlchemistMalorn()
            : base("The Alchemist of Shadows", "Malorn")
        {
        }

        public AlchemistMalorn(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Malorn's Alchemical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Malorn's Mystical Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Malorn's Enchanted Ring" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Malorn's Alchemist Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Malorn's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GoblinBloodCollectorQuest)
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
