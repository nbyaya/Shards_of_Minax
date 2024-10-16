using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class SweetDoughCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Sweet Dough Dilemma"; } }

        public override object Description
        {
            get
            {
                return "Ah, welcome, brave adventurer! I am Percival the Baker, and I find myself in dire need of your assistance. " +
                       "My legendary Sweet Dough recipe requires the finest Sweet Dough, and I'm short of 50 pieces. " +
                       "The dough is a crucial ingredient for my enchanted pastries, which I use to maintain the magical balance in our realm. " +
                       "If you help me collect these 50 Sweet Dough, I shall reward you handsomely with gold, a rare Maxxia Scroll, " +
                       "and a magnificent Baker's Ensemble that will make you the envy of all. Do you accept this task?";
            }
        }

        public override object Refuse { get { return "I understand. If you reconsider, please return with the Sweet Dough."; } }

        public override object Uncomplete { get { return "I still need 50 Sweet Dough. Please bring them to me so I can finish my magical pastries!"; } }

        public override object Complete { get { return "Fantastic! You've brought me the 50 Sweet Dough I required. Your help is immensely appreciated. " +
                       "As a token of my gratitude, please accept these rewards. May you continue to embark on extraordinary adventures!"; } }

        public SweetDoughCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SweetDough), "Sweet Dough", 50, 0x103d)); // Assuming Sweet Dough item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MacebearersLeggings), 1, "Baker's Ensemble")); // Assuming Baker's Ensemble is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Sweet Dough Collector quest!");
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

    public class BakerPercival : MondainQuester
    {
        [Constructable]
        public BakerPercival()
            : base("The Master Baker", "Percival")
        {
        }

        public BakerPercival(Serial serial)
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
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Percival's Baker Shirt" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Percival's Baker Kilt" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000), Name = "Percival's Baker Sandals" });
            AddItem(new HalfApron { Hue = Utility.Random(1, 3000), Name = "Percival's Enchanted Apron" });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Percival's Baker Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Percival's Magic Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Percival's Recipe Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SweetDoughCollectorQuest)
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
