using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class SulfurousAshCollectorQuest2 : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Sulfurous Ash Scribe"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave wanderer! I am Thandor, the Sulfurous Scribe. For eons, I have studied the arcane arts, " +
                       "and I require a rare component for my latest experiment: 500 units of Sulfurous Ash. This enchanted powder " +
                       "is essential for crafting a magical catalyst that will aid me in unlocking new realms of arcane knowledge. " +
                       "In exchange for your aid, I shall reward you with gold, a mystical Maxxia Scroll, and an ornate Chestguard of great significance.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me with the Sulfurous Ash."; } }

        public override object Uncomplete { get { return "I still require 50 Sulfurous Ash. Please bring them to me for my research."; } }

        public override object Complete { get { return "Splendid work! You have gathered the 50 Sulfurous Ash I needed. Your contribution will aid me in my magical pursuits. " +
                       "Please accept these rewards as a token of my gratitude. May your path be illuminated with success and fortune!"; } }

        public SulfurousAshCollectorQuest2() : base()
        {
            AddObjective(new ObtainObjective(typeof(SulfurousAsh), "Sulfurous Ash", 500, 0xF8C)); // Assuming Sulfurous Ash item ID is 0x1F6
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MarksmansLeatherChestguard), 1, "Sulfurous Scribe's Chestguard")); // Assuming Sulfurous Scribe's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Sulfurous Ash Scribe quest!");
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

    public class SulfurousScribeThandor : MondainQuester
    {
        [Constructable]
        public SulfurousScribeThandor()
            : base("The Sulfurous Scribe", "Thandor")
        {
        }

        public SulfurousScribeThandor(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body, change if needed

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Thandor's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Thandor's Arcane Hat" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Thandor's Enchanted Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thandor's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SulfurousAshCollectorQuest2)
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
