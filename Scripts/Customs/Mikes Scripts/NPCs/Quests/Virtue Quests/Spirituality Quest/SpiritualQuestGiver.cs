using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Items;

namespace Server.Mobiles
{
    public class SpiritualQuestGiver : MondainQuester
    {
        [Constructable]
        public SpiritualQuestGiver() : base("The Wise Sage")
        {
        }

        public SpiritualQuestGiver(Serial serial) : base(serial)
        {
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ActsOfSpiritualityQuest)
                };
            }
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 400; // Human Male
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Example hair style
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new Backpack());
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
