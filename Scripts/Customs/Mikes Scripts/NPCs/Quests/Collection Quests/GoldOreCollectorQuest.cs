using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GoldOreCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Alchemist's Gold Rush"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Master Alchemist Arion. My research into the ancient properties of gold requires " +
                       "a substantial amount of Gold Ore. Specifically, I need 50 pieces to complete my latest experiment. " +
                       "In exchange for your invaluable assistance, I will reward you with gold, a rare Maxxia Scroll, and a uniquely enchanted " +
                       "Alchemist's Tunic that will signify your achievement.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Gold Ore."; } }

        public override object Uncomplete { get { return "I still require 50 pieces of Gold Ore. Please bring them to me to assist with my research!"; } }

        public override object Complete { get { return "Fantastic work! You have brought me the 50 Gold Ore I needed. Your contribution is most appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be ever rewarding!"; } }

        public GoldOreCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(GoldOre), "Gold Ore", 50, 0x19B8)); // Assuming Gold Ore item ID is 0x19B8
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GrapplersTunic), 1, "Alchemist's Tunic")); // Assuming Alchemist's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Alchemist's Gold Rush quest!");
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

    public class MasterAlchemistArion : MondainQuester
    {
        [Constructable]
        public MasterAlchemistArion()
            : base("The Master Alchemist", "Arion")
        {
        }

        public MasterAlchemistArion(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203D; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest { Hue = Utility.Random(1, 3000), Name = "Arion's Alchemist Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Arion's Alchemist Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Arion's Enchanted Bracelet" });
            AddItem(new LeatherArms { Hue = Utility.Random(1, 3000), Name = "Arion's Mystical Circlet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Arion's Alchemical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GoldOreCollectorQuest)
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
