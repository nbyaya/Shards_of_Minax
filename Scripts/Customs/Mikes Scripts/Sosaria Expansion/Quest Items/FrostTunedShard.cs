using System;
using Server;

namespace Server.Items
{
    public class FrostTunedShard : Item
    {
        [Constructable]
        public FrostTunedShard() : base(0x1F19)
        {
            Name = "Frost-Tuned Shard";
            Hue = 1152;
            Weight = 1.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack to use.");
                return;
            }

            from.FixedParticles(0x375A, 10, 15, 5012, Hue, 0, EffectLayer.Head);
            from.PlaySound(0x64B);
            from.SendMessage("The shard hums. You feel the chill winds around you subside briefly.");

            // Optional: Apply temp weather buff or clear weather logic
        }

        public FrostTunedShard(Serial serial) : base(serial) { }

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
