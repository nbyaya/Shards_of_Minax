using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests
{
    public class DrRilloFane : MondainQuester
    {
        [Constructable]
        public DrRilloFane() : base("Technoscholar", "Dr. Rillo Fane")
        {
        }

        public override void InitBody()
        {
            InitStats(60, 80, 100);
            Body = 0x190;
            Hue = 0x83EA;
            HairItemID = 0x2049;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(2966)); // Cool techy hue
            AddItem(new Sandals(1152));
            AddItem(new HalfApron { Name = "Tech Tool Belt", Hue = 1150 });
        }

        public override Type[] Quests => new Type[]
        {
            typeof(ReclaimTheFutureQuest)
        };

        public DrRilloFane(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
