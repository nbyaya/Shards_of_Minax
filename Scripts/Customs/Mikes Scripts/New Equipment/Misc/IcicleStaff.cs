using System;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public class IcicleStaff : BaseStaff
    {
        [Constructable]
        public IcicleStaff() : base(0x2D25) // Use the appropriate item ID for the icicle staff graphic.
        {
            Weight = 6.0;
            Layer = Layer.TwoHanded;
            Name = "Icicle Staff";
        }

        public IcicleStaff(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack) || Parent == from)
            {
                from.BeginTarget(10, false, TargetFlags.Harmful, new TargetCallback(OnTarget));
                from.SendMessage("Who do you want to slow?");
            }
            else
            {
                from.SendMessage("The staff must be in your hand or pack to use.");
            }
        }

        public void OnTarget(Mobile from, object targeted)
        {
            if (targeted is Mobile)
            {
                Mobile target = (Mobile)targeted;

                // Apply a simple slowing effect
                if (target is BaseCreature)
                {
                    BaseCreature creature = (BaseCreature)target;
                    creature.ActiveSpeed *= 1.5; // Slow down by 50%
                    creature.PassiveSpeed *= 1.5; // Slow down by 50%
                }
                from.SendMessage("You've slowed the target with your Icicle Staff!");
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
