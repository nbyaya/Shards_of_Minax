using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Items;

namespace Server.Mobiles
{
    public class MirnaHollow : MondainQuester
    {
        [Constructable]
        public MirnaHollow() : base("Witch-Hunter", "Mirna Hollow")
        {
        }

        public MirnaHollow(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(85, 90, 80);
            Body = 0x191;
            NameHue = 1153;
            Hue = 2406; // Pale with subtle dark circles
            HairItemID = 0x203B;
            HairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1109));
            AddItem(new Cloak(1150));
            AddItem(new LeatherGloves(1109));
            AddItem(new Boots(1109));
            AddItem(new QuarterStaff());

            Backpack pack = new Backpack();
            pack.Name = "Witch-Hunter's Satchel";
            AddItem(pack);
        }

        public override Type[] Quests => new Type[]
        {
            typeof(TomesOfUndoingQuest)
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
