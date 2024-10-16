using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FeyWingCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Fey Wing Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurous soul! I am Eldara, the Fey Guardian. I need your help to collect 50 Fey Wings. " +
                       "These wings hold great magical significance and are crucial for my enchantments. In return for your assistance, " +
                       "I will reward you with gold, a rare Maxxia Scroll, and a uniquely enchanted Fey Guardian's Treads.";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, return to me with the Fey Wings."; } }

        public override object Uncomplete { get { return "I still require 50 Fey Wings. Please bring them to me to aid my magical endeavors!"; } }

        public override object Complete { get { return "Excellent work! You have brought me the 50 Fey Wings I needed. Your help is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your journeys be ever prosperous!"; } }

        public FeyWingCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(FeyWings), "Fey Wings", 50, 0x5726)); // Assuming Fey Wing item ID is 0xF5F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ShadowMastersTreads), 1, "Fey Guardian's Treads")); // Assuming Fey Guardian's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Fey Wing Collector quest!");
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

    public class FeyGuardianEldara : MondainQuester
    {
        [Constructable]
        public FeyGuardianEldara()
            : base("The Fey Guardian", "Eldara")
        {
        }

        public FeyGuardianEldara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Eldara's Fey Robe" });
            AddItem(new ThighBoots { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Eldara's Enchanted Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Eldara's Mystical Ring" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Eldara's Fey Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Eldara's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FeyWingCollectorQuest)
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
