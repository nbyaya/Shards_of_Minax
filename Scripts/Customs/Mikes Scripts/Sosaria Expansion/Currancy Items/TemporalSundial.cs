using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class TemporalSundial : Item
    {
        [Constructable]
        public TemporalSundial() : base(0x1042) // Graphic: Sundial
        {
            Name = "Temporal Sundial";
            Hue = 1161; // A mystical light blue hue
            Weight = 1.0;
        }

        public TemporalSundial(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendMessage("Select the magic map to twist its duration.");
            from.Target = new SundialTarget(this);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Twists the passage of time within a magical map.");
        }

        private class SundialTarget : Target
        {
            private readonly TemporalSundial _sundial;

            public SundialTarget(TemporalSundial sundial) : base(12, false, TargetFlags.None)
            {
                _sundial = sundial;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_sundial.Deleted)
                    return;

                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack to affect it.");
                        return;
                    }

                    // Flip a coin: halve or double
                    bool halve = Utility.RandomBool();

                    TimeSpan oldTime = map.ExpirationTime;
                    if (halve)
                    {
                        map.ExpirationTime = TimeSpan.FromMinutes(Math.Max(5, oldTime.TotalMinutes / 2));
                        from.SendMessage("The sands of time slip faster... the map will expire sooner!");
                    }
                    else
                    {
                        map.ExpirationTime = TimeSpan.FromMinutes(Math.Min(120, oldTime.TotalMinutes * 2));
                        from.SendMessage("The time-warp extends your map’s magic longer!");
                    }

                    _sundial.Delete(); // Consume the sundial
                }
                else
                {
                    from.SendMessage("That is not a magic map.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_sundial.Deleted)
                    from.SendMessage("You put the sundial away.");
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
