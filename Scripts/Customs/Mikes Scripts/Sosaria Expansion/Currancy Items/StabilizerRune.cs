using System;
using Server;
using Server.Items;
using Server.Targeting;
using System.Linq;
using System.Collections.Generic;    // for List<T>


namespace Server.Items
{
    public class StabilizerRune : Item
    {
        [Constructable]
        public StabilizerRune() : base(0x1F17) // Looks like a rune
        {
            Name = "Stabilizer Rune";
            Hue = 2101; // Ethereal/grey magic hue
            Weight = 1.0;
        }

        public StabilizerRune(Serial serial) : base(serial)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Flattens chaos into predictability.");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendMessage("Select the magic map you wish to stabilize.");
            from.Target = new RuneTarget(this);
        }

        private class RuneTarget : Target
        {
            private readonly StabilizerRune _rune;

            public RuneTarget(StabilizerRune rune) : base(12, false, TargetFlags.None)
            {
                _rune = rune;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_rune.Deleted)
                    return;

                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack to stabilize it.");
                        return;
                    }

                    if (!map.ActiveModifiers.Any())
                    {
                        from.SendMessage("This map is already stable—no modifiers to remove.");
                        return;
                    }

                    // Remove all modifiers
					var count = 0;
					foreach (var _ in map.ActiveModifiers)
						count++;

					// Clear the private list via reflection
					var field = map.GetType().GetField(
						"_activeModIds",
						System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
					);
					if (field != null)
						field.SetValue(map, new List<string>());

					// **Force the client to rebuild the tooltip**
					map.InvalidateProperties();

					from.SendMessage($"You stabilize the map and remove {count} modifier{(count == 1 ? "" : "s")}.");
					_rune.Delete();

                }
                else
                {
                    from.SendMessage("You can only use this on a magic map.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_rune.Deleted)
                    from.SendMessage("You decide not to use the Stabilizer Rune.");
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
