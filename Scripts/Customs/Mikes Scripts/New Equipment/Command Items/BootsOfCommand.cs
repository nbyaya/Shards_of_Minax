using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BootsOfCommand : BaseClothingOfCommand
    {
        [Constructable]
        public BootsOfCommand(int bonus) : base(bonus, 0x170B) // 0x170B is the ItemID for regular boots
        {
            Weight = 3.0;
            Name = "Boots of Command";
            Hue = Utility.RandomMinMax(1150, 1175);
            Layer = Layer.Shoes;
        }

        [Constructable]
        public BootsOfCommand() : this(1)
        {
        }

        public BootsOfCommand(Serial serial) : base(serial)
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
