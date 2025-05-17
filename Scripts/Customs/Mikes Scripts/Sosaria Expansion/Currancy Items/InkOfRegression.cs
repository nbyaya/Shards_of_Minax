using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class InkOfRegression : Item
    {
        [Constructable]
        public InkOfRegression() : base(0x0E24) // Bottle graphic
        {
            Name = "Ink of Regression";
            Hue = 2216; // Soft violet/purple, tweak if desired
            Weight = 1.0;
        }

        public InkOfRegression(Serial serial) : base(serial)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Smudges away the map’s most perilous challenges.");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendMessage("Select the magic map you wish to regress.");
            from.Target = new RegressionTarget(this);
        }

        private class RegressionTarget : Target
        {
            private readonly InkOfRegression _ink;

            public RegressionTarget(InkOfRegression ink) : base(12, false, TargetFlags.None)
            {
                _ink = ink;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_ink.Deleted)
                    return;

                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack to modify it.");
                        return;
                    }

                    if (map.Tier <= 1)
                    {
                        from.SendMessage("This map is already at the minimum tier.");
                        return;
                    }

                    map.Tier -= 1;
                    from.SendMessage($"The map’s magical power recedes. It is now Tier {map.Tier}.");
                    _ink.Delete();
                }
                else
                {
                    from.SendMessage("You can only use this ink on a magic map.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_ink.Deleted)
                    from.SendMessage("You decide not to use the ink.");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
