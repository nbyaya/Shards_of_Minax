using System;
using Server.Items;

namespace Server.Items
{
	
    public class DragonchainShard : Item
    {
        [Constructable]
        public DragonchainShard() : base(0x2FD4)
        {
            Hue = 1360;
            Name = "Shard of the Dragonchain";
            Weight = 0.5;
        }

        public DragonchainShard(Serial serial) : base(serial) { }

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
