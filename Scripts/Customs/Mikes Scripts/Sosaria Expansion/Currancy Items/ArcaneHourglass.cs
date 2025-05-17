using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class ArcaneHourglass : Item
    {
        [Constructable]
        public ArcaneHourglass() : base(0x1810) // hourglass graphic
        {
            Name = "Arcane Hourglass";
            Hue = 1150;
            Weight = 1.0;
        }

        public ArcaneHourglass(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendMessage("Select a magic map to extend its duration.");
            from.Target = new HourglassTarget(this);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Extends the magic map's expiration by 5 minutes.");
        }

        private class HourglassTarget : Target
        {
            private readonly ArcaneHourglass _hourglass;

            public HourglassTarget(ArcaneHourglass hourglass) : base(12, false, TargetFlags.None)
            {
                _hourglass = hourglass;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_hourglass.Deleted)
                    return;

                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack to modify it.");
                        return;
                    }

                    // Apply +5 minutes
                    map.ExpirationTime += TimeSpan.FromMinutes(5.0);
                    from.SendMessage("The sands of time shimmer. The map now lasts 5 minutes longer.");

                    _hourglass.Delete();
                }
                else
                {
                    from.SendMessage("You can only use the Arcane Hourglass on a magic map.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_hourglass.Deleted)
                    from.SendMessage("You decide not to use the hourglass.");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
