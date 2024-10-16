using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class JeweledFiligreeCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Jeweled Filigree Quest"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble adventurer! I am Seraphis, the Master Artificer. I seek your aid in a task of great importance. " +
                       "I require 50 Jeweled Filigree to complete a grand masterpiece. These filigrees are crucial for enchanting artifacts of great power. " +
                       "In return for your aid, you will be rewarded with gold, a rare Maxxia Scroll, and a one-of-a-kind Artificer's Attire, " +
                       "woven with magical threads that shimmer with the hues of the realm.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Jeweled Filigree."; } }

        public override object Uncomplete { get { return "I still require 50 Jeweled Filigree. Please gather them so I can complete my masterpiece!"; } }

        public override object Complete { get { return "Excellent work! You have gathered the 50 Jeweled Filigree I needed. Your contribution is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be as dazzling as the filigree you have brought me!"; } }

        public JeweledFiligreeCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(JeweledFiligree), "Jeweled Filigree", 50, 0x2F5E)); // Assuming Jeweled Filigree item ID is 0x1F6
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MasterTinkerersHelmet), 1, "Artificer's Attire")); // Assuming Artificer's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Jeweled Filigree Quest!");
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

    public class ArtificerSeraphis : MondainQuester
    {
        [Constructable]
        public ArtificerSeraphis()
            : base("The Master Artificer", "Seraphis")
        {
        }

        public ArtificerSeraphis(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 40);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203E; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Seraphis' Artificer's Chest" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Seraphis' Enchanted Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Seraphis' Magical Ring" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Seraphis' Enchanted Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Seraphis' Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(JeweledFiligreeCollectorQuest)
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
