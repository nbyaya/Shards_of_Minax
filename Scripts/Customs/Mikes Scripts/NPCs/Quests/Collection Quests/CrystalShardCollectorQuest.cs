using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CrystalShardCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Crystal Sage's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave soul! I am Glenwood, the Crystal Sage. I seek your help to gather 50 Crystal Shards, " +
                       "for they hold great power and are essential for my arcane studies. Your assistance in this matter would be " +
                       "greatly appreciated. In return, I shall reward you with gold, a rare Maxxia Scroll, and a magical Crystal Robe " +
                       "that enhances your mystical aura.";
            }
        }

        public override object Refuse { get { return "Very well. If you reconsider, feel free to return to me with the shards."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Crystal Shards. Please bring them to me so that I may continue my research!"; } }

        public override object Complete { get { return "Excellent work! You have brought me the 50 Crystal Shards I needed. Your aid has been invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May the crystals guide you on your journey!"; } }

        public CrystalShardCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CrystalShards), "Crystal Shard", 50, 0x5738)); // Assuming Crystal Shard item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SeductressSilkenShoes), 1, "Magical Crystal Shoes")); // Assuming Crystal Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Crystal Sage's Request quest!");
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

    public class CrystalSageGlenwood : MondainQuester
    {
        [Constructable]
        public CrystalSageGlenwood()
            : base("The Crystal Sage", "Glenwood")
        {
        }

        public CrystalSageGlenwood(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203D; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Glenwood's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Glenwood's Crystal Hat" });
            AddItem(new GnarledStaff { Hue = Utility.Random(1, 3000), Name = "Glenwood's Enchanted Staff" }); // Assuming Crystal Staff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Glenwood's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CrystalShardCollectorQuest)
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
