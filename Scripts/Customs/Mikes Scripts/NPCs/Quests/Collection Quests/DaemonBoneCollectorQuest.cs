using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class DaemonBoneCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Daemon Bone Collector's Challenge"; } }

        public override object Description
        {
            get
            {
                return "Greetings, valiant adventurer! I am Malakar, the Daemon Bone Collector. I need your assistance to gather 50 Daemon Bones, " +
                       "for they are crucial to my dark experiments. Your help in this matter will be greatly rewarded. I shall provide you with gold, " +
                       "a rare Maxxia Scroll, and a mystical Daemon Bone Armor set in gratitude.";
            }
        }

        public override object Refuse { get { return "Very well. If you reconsider, you can always come back with the Daemon Bones."; } }

        public override object Uncomplete { get { return "I still need 50 Daemon Bones. Please bring them to me so that I can proceed with my work!"; } }

        public override object Complete { get { return "Excellent! You have brought me the 50 Daemon Bones I required. Your assistance is invaluable. " +
                       "As a token of my gratitude, please accept these rewards. May the dark powers guide you on your path!"; } }

        public DaemonBoneCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DaemonBone), "Daemon Bone", 50, 0xF80)); // Assuming Daemon Bone item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MaestrosDo), 1, "Mystical Daemon Bone Armor")); // Assuming Daemon Bone Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Daemon Bone Collector's Challenge quest!");
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

    public class DaemonBoneCollectorMalakar : MondainQuester
    {
        [Constructable]
        public DaemonBoneCollectorMalakar()
            : base("The Daemon Bone Collector", "Malakar")
        {
        }

        public DaemonBoneCollectorMalakar(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2042; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Malakar's Dark Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Malakar's Shadow Hat" });
            AddItem(new GnarledStaff { Hue = Utility.Random(1, 3000), Name = "Malakar's Infernal Staff" }); // Assuming Daemon Staff is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Malakar's Cursed Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DaemonBoneCollectorQuest)
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
