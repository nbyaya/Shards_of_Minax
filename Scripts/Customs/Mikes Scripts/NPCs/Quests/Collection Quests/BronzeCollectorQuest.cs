using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class BronzeCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Bronze Smith's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! Our village is in dire need of bronze ingots to repair our tools and weapons. " +
                       "If you could bring me 50 bronze ingots, I will make sure you are well rewarded!";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, just let me know."; } }

        public override object Uncomplete { get { return "You haven't collected enough bronze ingots yet. Keep at it!"; } }

        public override object Complete { get { return "Excellent work! Here is your reward for helping us gather the bronze ingots."; } }

        public BronzeCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BronzeIngot), "Bronze Ingots", 50, 0x1BF2));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(BeastmastersTonic), 1, "Smith's Tunic"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Bronze Smith's Request!");
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

    public class BronzeSmithGavin : MondainQuester
    {
        [Constructable]
        public BronzeSmithGavin()
            : base("The Smith", "Bronze Smith Gavin")
        {
        }

        public BronzeSmithGavin(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(90, 90, 30);

            Body = 0x190; // Male Body
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2044; // Stylish hair style
            HairHue = 0x4F; // Hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = 0x8A5 }); // Bronze-colored armor
            AddItem(new PlateLegs { Hue = 0x8A5 });
            AddItem(new PlateGloves { Hue = 0x8A5 });
            AddItem(new PlateHelm { Hue = 0x8A5 });
            AddItem(new SmithHammer { Name = "Gavin's Smithing Hammer", Hue = 0 });
            AddItem(new Boots(0x8A5));
            AddItem(new Backpack { Hue = 0x8A5, Name = "Smith's Tool Kit" });
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BronzeCollectorQuest)
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
