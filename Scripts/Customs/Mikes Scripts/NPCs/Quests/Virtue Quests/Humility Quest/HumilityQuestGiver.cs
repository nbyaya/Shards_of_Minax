using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class HumilityQuestGiver : MondainQuester
    {
        [Constructable]
        public HumilityQuestGiver() : base("The Humble Monk")
        {
        }

        public HumilityQuestGiver(Serial serial) : base(serial)
        {
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ActsOfHumilityQuest)
                };
            }
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Body = 400; // Human Male
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2048; // Example hair style
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Sandals());
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
