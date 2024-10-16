using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class GlovesOfCommand : BaseClothingOfCommand
    {
        [Constructable]
        public GlovesOfCommand(int bonus) : base(bonus, 0x1450)  // 0x1450 is the item ID for leather gloves
        {
            Weight = 1.0;
            Name = "Gloves of Command";
            Hue = Utility.RandomMinMax(1150, 1175);
            Layer = Layer.Gloves;
        }

        [Constructable]
        public GlovesOfCommand() : this(1)
        {
        }

        public GlovesOfCommand(Serial serial) : base(serial)
        {
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
