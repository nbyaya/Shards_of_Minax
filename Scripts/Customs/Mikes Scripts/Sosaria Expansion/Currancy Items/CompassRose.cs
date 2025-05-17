using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class CompassRose : Item
    {
        [Constructable]
        public CompassRose() : base(0x1F14) // You can change this graphic if desired
        {
            Name = "Compass Rose";
            Hue = 1150;
            Weight = 1.0;
        }

        public CompassRose(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendMessage("Select the magic map whose location you wish to reroll.");
            from.Target = new CompassRoseTarget(this);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Realigns the cartographic focus.");
        }

        private class CompassRoseTarget : Target
        {
            private readonly CompassRose _rose;

            public CompassRoseTarget(CompassRose rose) : base(12, false, TargetFlags.None)
            {
                _rose = rose;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_rose.Deleted)
                    return;

                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack to modify it.");
                        return;
                    }

                    // Regenerate predefined locations
                    if (map.PredefinedLocations != null && map.PredefinedLocations.Count > 0)
                    {
                        map.PredefinedLocations.Clear();
                    }

                    // Add fresh randomized locations (this logic can be customized)
                    for (int i = 0; i < 5; i++)
                    {
                        var x = Utility.Random(1000, 3000);
                        var y = Utility.Random(1000, 3000);
                        var z = 0;
                        map.PredefinedLocations.Add(new Point3D(x, y, z));
                    }

                    from.SendMessage("The map's destination has shifted to an unknown place...");
                    _rose.Delete();
                }
                else
                {
                    from.SendMessage("You can only use the Compass Rose on a magic map.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_rose.Deleted)
                    from.SendMessage("You decide not to change any map locations.");
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
