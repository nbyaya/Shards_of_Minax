using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class SingularityRune : Item
	{
		[Constructable]
		public SingularityRune() : base(0x1F23)
		{
			Name = "Singularity Rune";
			Hue  = 0x5A6; // dark purple
			Weight = 1.0;
		}

		public SingularityRune(Serial s) : base(s) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}
			from.SendMessage("Target the map you wish to simplify.");
			from.Target = new RuneTarget(this);
		}

		private class RuneTarget : Target
		{
			private readonly SingularityRune _rune;
			public RuneTarget(SingularityRune rune) : base(12, false, TargetFlags.None)
			{
				_rune = rune;
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				if (_rune.Deleted) return;

				if (targ is MagicMapBase map && map.IsChildOf(from.Backpack))
				{
					map.ClearPredefinedLocations();

					var loc = new Point3D(
						from.X + Utility.RandomMinMax(-50, 50),
						from.Y + Utility.RandomMinMax(-50, 50),
						from.Z);

					map.AddPredefinedLocation(loc);

					from.SendMessage("The map now leads to a singular location... nearby.");
					_rune.Delete();
				}
				else
				{
					from.SendMessage("That is not a valid magic map.");
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
