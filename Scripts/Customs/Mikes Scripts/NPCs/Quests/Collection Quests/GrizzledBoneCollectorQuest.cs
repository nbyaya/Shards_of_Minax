using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GrizzledBoneCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Grizzled Bone Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Taryn, the Bone Collector. Long ago, a mighty beast roamed these lands, " +
                       "leaving behind remnants of its power in the form of Grizzled Bones. I need your assistance to gather 50 of these " +
                       "bones. They are crucial for my studies on ancient beasts and their magical properties. In exchange for your " +
                       "hard work, I shall reward you with gold, a rare Maxxia Scroll, and a unique Grizzled Collector's Outfit.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Grizzled Bones."; } }

        public override object Uncomplete { get { return "I still require 50 Grizzled Bones. Please bring them to me to help with my research!"; } }

        public override object Complete { get { return "Fantastic! You've brought me the 50 Grizzled Bones I requested. Your dedication is truly commendable. " +
                       "Please accept these rewards as a token of my appreciation. May your journey be filled with adventure!"; } }

        public GrizzledBoneCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(GrizzledBones), "Grizzled Bones", 50, 0x318C)); // Assuming Grizzled Bone item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(TimberlordsHelm), 1, "Grizzled Collector's Outfit")); // Assuming Grizzled Collector's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Grizzled Bone Hunt quest!");
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

    public class GrizzledBoneCollectorTaryn : MondainQuester
    {
        [Constructable]
        public GrizzledBoneCollectorTaryn()
            : base("The Bone Collector", "Taryn")
        {
        }

        public GrizzledBoneCollectorTaryn(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new BoneChest { Hue = Utility.Random(1, 3000), Name = "Taryn's Bone Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new BoneHelm { Hue = Utility.Random(1, 3000), Name = "Taryn's Collector's Hat" });
            AddItem(new BoneGloves { Hue = Utility.Random(1, 3000), Name = "Taryn's Bone Gloves" });
            AddItem(new BoneLegs { Hue = Utility.Random(1, 3000), Name = "Taryn's Bone Leggings" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Taryn's Collection Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GrizzledBoneCollectorQuest)
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
