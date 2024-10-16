using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FlaxCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Flax Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Thalorin, the Weaver of Wonders. I need your assistance to gather 50 Flax. " +
                       "These fibers are essential for crafting enchanted textiles. In return for your effort, you shall receive gold, " +
                       "a rare Maxxia Scroll, and a special Bandana crafted from the finest magical threads.";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, return to me with the Flax."; } }

        public override object Uncomplete { get { return "I still need 50 Flax. Please bring them to me so I can continue my weaving work!"; } }

        public override object Complete { get { return "Fantastic! You've brought me the 50 Flax I needed. Your help is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your travels be prosperous!"; } }

        public FlaxCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Flax), "Flax", 50, 0x1A9C)); // Assuming Flax item ID is 0x1C1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GrungeBandana), 1, "Thalorin's Weavers Bandana")); // Assuming Weavers Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Flax Collector quest!");
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

    public class WeaverThalorin : MondainQuester
    {
        [Constructable]
        public WeaverThalorin()
            : base("The Weaver of Wonders", "Thalorin")
        {
        }

        public WeaverThalorin(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Thalorin's Magical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Thalorin's Enchanted Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Spellbook { Hue = Utility.Random(1, 3000), Name = "Thalorin's Weaving Tome" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thalorin's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FlaxCollectorQuest)
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
