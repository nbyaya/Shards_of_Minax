using System;
using Server.Targeting;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class OneHandedTransformTarget : Target
    {
        private OneHandedTransformDeed m_Deed;

        public OneHandedTransformTarget(OneHandedTransformDeed deed) : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (m_Deed.Deleted || m_Deed.RootParent != from)
            {
                from.SendMessage("That deed has been removed.");
                return;
            }

            if (target is BaseWeapon)
            {
                BaseWeapon weapon = (BaseWeapon)target;
                if (weapon.Layer == Layer.TwoHanded)
                {
                    weapon.Layer = Layer.OneHanded;
                    from.SendMessage("Your weapon has been transformed into a one-handed weapon.");
                    m_Deed.Delete();
                }
                else
                {
                    from.SendMessage("This can only be used on two-handed weapons.");
                }
            }
            else
            {
                from.SendMessage("That's not a weapon.");
            }
        }
    }

    public class OneHandedTransformDeed : Item
    {
        [Constructable]
        public OneHandedTransformDeed() : base(0x14F0)
        {
            Name = "a one-handed transform deed";
        }

        public OneHandedTransformDeed(Serial serial) : base(serial) { }

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

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("The deed must be in your backpack to use.");
                return;
            }

            from.SendMessage("Select the two-handed weapon you wish to transform into a one-handed weapon.");
            from.Target = new OneHandedTransformTarget(this);
        }
    }
}
