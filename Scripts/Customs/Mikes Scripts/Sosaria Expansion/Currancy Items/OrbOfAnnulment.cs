using System;
using Server;
using Server.Items;
using Server.Targeting;
using System.Linq;          // ← add this!

namespace Server.Items
{
	public class OrbOfAnnulment : Item
	{
		[Constructable]
		public OrbOfAnnulment() : base(0x186B)
		{
			Name   = "Orb of Annulment";
			Hue    = 0x48C;
			Weight = 1.0;
		}

		public OrbOfAnnulment(Serial serial) : base(serial) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Removes a random moddifier");
        }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}

			from.SendMessage("Select the magic map you wish to purge.");
			from.Target = new PurgeTarget(this);
		}

		private class PurgeTarget : Target
		{
			private readonly OrbOfAnnulment _glyph;
			public PurgeTarget(OrbOfAnnulment glyph) : base(12, false, TargetFlags.None)
			{
				_glyph = glyph;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (_glyph.Deleted) return;

				if (targeted is MagicMapBase map && map.IsChildOf(from.Backpack))
				{
					if (!map.ActiveModifiers.Any())
					{
						from.SendMessage("That map has no modifiers to remove.");
					}
					else
					{
						// pick one to remove, then give feedback
						var removed = map.ActiveModifiers
										 .OrderBy(_=>Utility.RandomDouble())
										 .First();
						map.RemoveModifier(removed.ID);
						from.SendMessage($"The glyph flares—“{removed.Name}” has been banished!");
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
			writer.Write(0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			reader.ReadInt();
		}
	}

}
