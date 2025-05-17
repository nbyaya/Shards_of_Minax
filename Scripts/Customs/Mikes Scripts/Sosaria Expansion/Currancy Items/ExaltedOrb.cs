using System;
using Server;
using Server.Items;
using Server.Targeting;
using System.Linq;          // ← add this!

namespace Server.Items
{
	public class ExaltedOrb : Item
	{
		[Constructable]
		public ExaltedOrb() : base(0x186A)
		{
			Name   = "Exalted Orb";
			Hue    = 0x4AA;
			Weight = 1.0;
		}

		public ExaltedOrb(Serial serial) : base(serial) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Adds a random moddifer");
        }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}

			from.SendMessage("Select the magic map you wish to empower.");
			from.Target = new AmplifyTarget(this);
		}

		private class AmplifyTarget : Target
		{
			private readonly ExaltedOrb _glyph;
			public AmplifyTarget(ExaltedOrb glyph) : base(12, false, TargetFlags.None)
			{
				_glyph = glyph;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (_glyph.Deleted) return;

				if (targeted is MagicMapBase map && map.IsChildOf(from.Backpack))
				{
					// build list of available
					var available = MapModifierRegistry.All
									  .Where(m => !map.ActiveModifiers.Any(am => am.ID == m.ID))
									  .ToList();

					if (available.Count == 0)
					{
						from.SendMessage("That map already has every possible modifier.");
					}
					else
					{
						var mod = available[Utility.Random(available.Count)];
						map.AddModifier(mod.ID);
						from.SendMessage($"Arcane runes swirl—“{mod.Name}” has been added!");
						_glyph.Delete();
					}
				}
				else
				{
					from.SendMessage("That’s not a magic map in your pack.");
				}
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
			reader.ReadInt();
		}
	}

}
