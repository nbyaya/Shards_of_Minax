using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DiamondCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Diamond Collector's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Eldric, the Diamond Collector. I require your aid in gathering 50 Diamonds, " +
                       "which are crucial for my current project. Your efforts in bringing these diamonds to me will be rewarded with gold, " +
                       "a rare Maxxia Scroll, and a magnificent Diamond Collector's Ensemble, crafted to showcase your dedication.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, come back with the diamonds."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Diamonds. Please bring them to me so that I may complete my project!"; } }

        public override object Complete { get { return "Excellent! You've collected the 50 Diamonds I requested. Your assistance is greatly appreciated. " +
                       "As a token of my gratitude, please accept these rewards. May your adventures be filled with prosperity!"; } }

        public DiamondCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Diamond), "Diamond", 50, 0xF26)); // Assuming Diamond item ID is 0x1F1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ChampagneToastTunic), 1, "Diamond Collector's Ensemble")); // Custom item for the quest reward
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Diamond Collector's Request quest!");
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

    public class DiamondCollectorEldric : MondainQuester
    {
        [Constructable]
        public DiamondCollectorEldric()
            : base("The Diamond Collector", "Eldric")
        {
        }

        public DiamondCollectorEldric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Eldric's Diamond Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new Circlet { Hue = Utility.Random(1, 3000), Name = "Eldric's Diamond Crown" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Eldric's Diamond Bracelet" });
            AddItem(new Longsword { Hue = Utility.Random(1, 3000), Name = "Eldric's Diamond Sword" }); // Assuming Diamond Sword is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Eldric's Treasure Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DiamondCollectorQuest)
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

    public class DiamondCollectorEnsemble : Item
    {
        [Constructable]
        public DiamondCollectorEnsemble() : base(0x1F00) // Example item ID
        {
            Name = "Diamond Collector's Ensemble";
            Hue = Utility.Random(1, 3000);
        }

        public DiamondCollectorEnsemble(Serial serial) : base(serial)
        {
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
