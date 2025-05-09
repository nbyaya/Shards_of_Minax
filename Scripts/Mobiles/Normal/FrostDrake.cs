using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drake corpse")]
    public class FrostDrake : ColdDrake
    {
        [Constructable]
        public FrostDrake()
        {
            Name = "a cold drake";
        }

        public FrostDrake(Serial serial)
            : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new FrostDrakeCloak());
            }
		}

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