using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class CartographersPen : Item
    {
        [Constructable]
        public CartographersPen() : base(0x0FBF)
        {
            Name = "Cartographer's Pen";
            Hue  = 1150;
            Weight = 1.0;
        }

        public CartographersPen(Serial serial) : base(serial)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Upgrades Magic Maps by +1 tier");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendMessage("Select the magic map you wish to upgrade.");
            from.Target = new PenTarget(this);
        }

        private class PenTarget : Target
        {
            private readonly CartographersPen _pen;

            public PenTarget(CartographersPen pen) : base(12, false, TargetFlags.None)
            {
                _pen = pen;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                // If the pen has been consumed already, do nothing
                if (_pen.Deleted)
                    return;

                // Must be a MagicMapBase
                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack to upgrade it.");
                        return;
                    }

                    if (map.Tier >= 16)
                    {
                        from.SendMessage("This map is already at the maximum tier.");
                        return;
                    }

                    // Upgrade
                    map.Tier += 1;
                    from.SendMessage($"The map’s power surges! It is now Tier {map.Tier}.");

                    // Consume the pen
                    _pen.Delete();
                }
                else
                {
                    from.SendMessage("You can only use the Cartographer's Pen on a magic map.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_pen.Deleted)
                    from.SendMessage("You decide not to modify any maps.");
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
