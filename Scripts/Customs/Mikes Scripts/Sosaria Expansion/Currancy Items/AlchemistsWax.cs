using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class AlchemistsWax : Item
    {
        [Constructable]
        public AlchemistsWax() : base(0x142B) // Using Wax graphic
        {
            Name = "Alchemist's Wax";
            Hue = 1266;
            Weight = 1.0;
        }

        public AlchemistsWax(Serial serial) : base(serial)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Preserves fragile artifacts inside.");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendMessage("Select the magic map you wish to enhance with extra treasure.");
            from.Target = new WaxTarget(this);
        }

        private class WaxTarget : Target
        {
            private readonly AlchemistsWax _wax;

            public WaxTarget(AlchemistsWax wax) : base(12, false, TargetFlags.None)
            {
                _wax = wax;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_wax.Deleted)
                    return;

                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack to modify it.");
                        return;
                    }

                    if (map.MaxChests >= 100)
                    {
                        from.SendMessage("This map cannot contain any more treasure chests.");
                        return;
                    }

                    map.MaxChests += 1;
                    from.SendMessage($"You seal the map with wax. It will now spawn {map.MaxChests} treasure chests.");
                    _wax.Delete();
                }
                else
                {
                    from.SendMessage("You can only use this on a magic map.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_wax.Deleted)
                    from.SendMessage("You decide not to use the wax.");
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
