using System;
using Server;

namespace Server.Items
{
    public class DivingSuit : BaseArmor
    {
        [Constructable]
        public DivingSuit() : base(0x1F03)
        {
            Name = "Diving Suit";
            Hue = 0x4F2;
            Weight = 15.0;
            
            ArmorAttributes.SelfRepair = 3;
            Attributes.NightSight = 1;
            Attributes.LowerRegCost = 15;
            
        }

        public DivingSuit(Serial serial) : base(serial)
        {
        }

        public override int BasePhysicalResistance { get { return 3; } }
        public override int BaseFireResistance { get { return 3; } }
        public override int BaseColdResistance { get { return 90; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 3; } }

        public override int InitMinHits { get { return 35; } }
        public override int InitMaxHits { get { return 45; } }

        public override int AosStrReq { get { return 25; } }

        public override int OldStrReq { get { return 10; } }

        public override int ArmorBase { get { return 30; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }

        public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

        public override void OnDoubleClick(Mobile from)
        {
            if (Parent == from)
            {
                from.SendMessage("You feel more comfortable in the water while wearing this suit.");
            }
            else
            {
                base.OnDoubleClick(from);
            }
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