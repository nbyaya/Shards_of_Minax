using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class SpiderCarapaceCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Spider Carapace Connoisseur"; } }

        public override object Description
        {
            get
            {
                return "Ah, a brave soul! I am Elrion, the Keeper of the Web. My study of the ancient spiders has led me to seek out " +
                       "50 Spider Carapaces. These carapaces are imbued with the essence of the arachnid and are crucial for my research " +
                       "into their mystical properties. In return for your efforts, I shall reward you with gold, a rare Maxxia Scroll, " +
                       "and an enchanted Tunic reflecting the spider's essence.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, come back with the Spider Carapaces."; } }

        public override object Uncomplete { get { return "I still need 50 Spider Carapaces. Please bring them to me so I can continue my studies!"; } }

        public override object Complete { get { return "Fantastic! You have gathered the 50 Spider Carapaces. Your assistance is invaluable. Here are your rewards: " +
                       "gold, a Maxxia Scroll, and the Spider Keeper's Robe. May your path be as stealthy and silent as a spider's stride!"; } }

        public SpiderCarapaceCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SpiderCarapace), "Spider Carapaces", 50, 0x5720)); // Assuming Spider Carapace item ID is 0x1C0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MinstrelsTunedTunic), 1, "Spider Keeper's Tunic")); // Assuming Spider Keeper's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations on completing the Spider Carapace Connoisseur quest!");
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

    public class SpiderKeeperElrion : MondainQuester
    {
        [Constructable]
        public SpiderKeeperElrion()
            : base("The Spider Keeper", "Elrion")
        {
        }

        public SpiderKeeperElrion(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2045; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Elrion's Spider Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Elrion's Spider Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Elrion's Spider Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Elrion's Spider Kilt" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Elrion's Webbed Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SpiderCarapaceCollectorQuest)
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
