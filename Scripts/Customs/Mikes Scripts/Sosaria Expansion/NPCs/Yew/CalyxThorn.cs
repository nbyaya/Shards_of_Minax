using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests
{
    public class CalyxThorn : MondainQuester
    {
        [Constructable]
        public CalyxThorn()
            : base("Calyx Thorn", "Shadow-Watcher of Yew")
        {
        }

        public CalyxThorn(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = true;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C;
            HairHue = 1150; // Dark purple
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows());
            AddItem(new Boots());
            AddItem(new Lantern());
        }

        public override Type[] Quests => new Type[]
        {
            typeof(SilenceTheChantQuest)
        };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
