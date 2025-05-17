using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	// ── 1) Radius Rune — expands the danger zone ─────────────────────────
	public class RadiusRune : Item
	{
		[Constructable]
		public RadiusRune() : base(0x0F91)
		{
			Name = "Rune of Expansion";
			Hue  = 0x48F;
			Weight = 1.0;
		}

		public RadiusRune(Serial s) : base(s) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}

			from.SendMessage("Select the magic map you wish to expand.");
			from.Target = new InternalTarget(this);
		}

		private class InternalTarget : Target
		{
			private readonly RadiusRune _rune;
			public InternalTarget(RadiusRune rune) : base(12, false, TargetFlags.None)
			{
				_rune = rune;
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				if (_rune.Deleted) return;

				if (targ is MagicMapBase map && map.IsChildOf(from.Backpack))
				{
					map.AdjustSpawnRadius(20);
					from.SendMessage("The map’s borders shift outward!");
					_rune.Delete();
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
