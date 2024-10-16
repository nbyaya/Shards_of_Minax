using System;
using Server;

namespace Server.Items
{
    public class EvolutionaryTome : Item
    {
        [Constructable]
        public EvolutionaryTome() : base(0x1C12)
        {
            Name = "Evolutionary Tome";
            Hue = 0x48D;
            Weight = 1.0;
            LootType = LootType.Regular;
        }

        public EvolutionaryTome(Serial serial) : base(serial)
        {
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
