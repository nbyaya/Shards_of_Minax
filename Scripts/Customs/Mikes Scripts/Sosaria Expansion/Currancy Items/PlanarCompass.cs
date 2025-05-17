using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class PlanarCompass : Item
	{
		[Constructable]
		public PlanarCompass() : base(0x0F7F)
		{
			Name = "Planar Compass";
			Hue  = 0x489;
			Weight = 1.0;
		}

		public PlanarCompass(Serial s) : base(s) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}
			from.SendMessage("Target the magic map whose plane you wish to shift.");
			from.Target = new CompassTarget(this);
		}

		private class CompassTarget : Target
		{
			private readonly PlanarCompass _compass;
			public CompassTarget(PlanarCompass compass) : base(12, false, TargetFlags.None)
			{
				_compass = compass;
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				if (_compass.Deleted) return;
				if (targ is MagicMapBase map && map.IsChildOf(from.Backpack))
				{
					// toggle between Felucca and Trammel for example
					var newMap = (map.DestinationFacet == Map.Felucca) ? Map.Trammel : Map.Felucca;
					map.SetDestinationFacet(newMap);
					from.SendMessage($"Arcane currents twist—the map now leads to {newMap}.");
					_compass.Delete();
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
