using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class JesterHatOfCommand : BaseClothingOfCommand
    {
        [Constructable]
        public JesterHatOfCommand(int bonus) : base(bonus, 0x171C) // 0x171C is the itemID for a jester hat
        {
            Weight = 2.0;
            Name = "Jester Hat of Command";
            Hue = Utility.RandomMinMax(1150, 1175);
            Layer = Layer.Helm;
        }

        [Constructable]
        public JesterHatOfCommand() : this(1)
        {
        }

        public JesterHatOfCommand(Serial serial) : base(serial)
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
