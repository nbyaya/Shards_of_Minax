using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class PlateLeggingsOfCommand : BaseClothingOfCommand
    {
        [Constructable]
        public PlateLeggingsOfCommand(int bonus) : base(bonus, 0x1411) // 0x141A for a different orientation
        {
            Weight = 7.0;
            Name = "Plate Leggings of Command";
            Hue = Utility.RandomMinMax(1150, 1175);
            Layer = Layer.Pants;  // Layer for pants/leggings
        }

        [Constructable]
        public PlateLeggingsOfCommand() : this(1)
        {
        }

        public PlateLeggingsOfCommand(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);  // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
