using Server;
using Server.Items;
using System;

namespace Server.Items
{
    public class TamersSack : Bag
    {
        [Constructable]
        public TamersSack()
        {
            Name = "Tamer's Sack";

            // Add 30 MasterBalls
            for (int i = 0; i < 30; i++)
            {
                DropItem(new MasterBall());
            }
        }

        public TamersSack(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
