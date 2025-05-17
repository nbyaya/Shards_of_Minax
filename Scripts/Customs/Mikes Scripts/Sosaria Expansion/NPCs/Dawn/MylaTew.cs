using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests
{
    public class MylaTew : MondainQuester
    {
        [Constructable]
        public MylaTew()
            : base("Apothecary", "Myla Tew")
        {
        }

        public MylaTew(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(60, 70, 80);
            Body = 401; // Female
            Hue = 0;
            HairItemID = 0x203B;
            HairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1150));
            AddItem(new Boots(0x497));
            AddItem(new HalfApron(1109));
            AddItem(new Lantern { Name = "Lumen of Dawn", Hue = 1278 });

            Backpack pack = new Backpack();
            pack.Name = "Apothecary's Satchel";
            AddItem(pack);
        }

        public override Type[] Quests => new Type[]
        {
            typeof(SporesForTheCureQuest)
        };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
