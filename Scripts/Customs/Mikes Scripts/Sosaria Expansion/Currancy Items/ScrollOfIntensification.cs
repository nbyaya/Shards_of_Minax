using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class ScrollOfIntensification : Item
    {
        [Constructable]
        public ScrollOfIntensification() : base(0x1F4C) // Example graphic: Scroll
        {
            Name = "Scroll of Intensification";
            Hue = 1358;
            Weight = 1.0;
        }

        public ScrollOfIntensification(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendMessage("Select the magic map you wish to intensify.");
            from.Target = new IntensifyTarget(this);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Doubles the number of monsters spawned by a magic map.");
        }

        private class IntensifyTarget : Target
        {
            private readonly ScrollOfIntensification _scroll;

            public IntensifyTarget(ScrollOfIntensification scroll) : base(12, false, TargetFlags.None)
            {
                _scroll = scroll;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_scroll.Deleted)
                    return;

                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack to modify it.");
                        return;
                    }

                    // Double the monster spawn count (but cap it reasonably to avoid server lag)
                    map.MaxMonsters = Math.Min(map.MaxMonsters * 2, 100);
                    from.SendMessage("The map pulses with energy. Its challenges grow fiercer!");

                    _scroll.Delete();
                }
                else
                {
                    from.SendMessage("You can only use the scroll on a magic map.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_scroll.Deleted)
                    from.SendMessage("You put the scroll away without using it.");
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
