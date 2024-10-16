using System;
using Server.Items;

namespace Server.Items
{
    public class GoldenDrakelingScaleShield : BaseShield
    {
        public override int BasePhysicalResistance
        {
            get { return 5; }
        }

        public override int BaseFireResistance
        {
            get { return 50; }
        }

        public override int BaseColdResistance
        {
            get { return 5; }
        }

        public override int BasePoisonResistance
        {
            get { return 5; }
        }

        public override int BaseEnergyResistance
        {
            get { return 5; }
        }

        public override int InitMinHits
        {
            get { return 255; }
        }

        public override int InitMaxHits
        {
            get { return 255; }
        }

        public override int AosStrReq
        {
            get { return 90; }
        }

        [Constructable]
        public GoldenDrakelingScaleShield() : base(0x1B76)
        {
            Weight = 6.0;
            Hue = 0x501;
            Name = "Golden Drakeling Scale Shield";
        }

        public GoldenDrakelingScaleShield(Serial serial) : base(serial)
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
