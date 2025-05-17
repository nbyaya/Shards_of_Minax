using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class CartographersPin : Item
	{
		[Constructable]
		public CartographersPin() : base(0x104F)
		{
			Name = "Cartographer’s Pin";
			Hue  = 0x55B;
			Weight = 0.5;
		}

		public CartographersPin(Serial s) : base(s) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}
			from.SendMessage("Target the map where you wish to add a secret location.");
			from.Target = new PinTarget(this);
		}

		private class PinTarget : Target
		{
			private readonly CartographersPin _pin;
			public PinTarget(CartographersPin pin) : base(12, false, TargetFlags.None)
			{
				_pin = pin;
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				if (_pin.Deleted) return;
				if (targ is MagicMapBase map && map.IsChildOf(from.Backpack))
				{
					// use caster’s current location as the new point
					map.ClearPredefinedLocations();
					var pt = from.Location;
					map.AddPredefinedLocation(pt);
					from.SendMessage($"A new rendezvous has been charted at {pt}.");
					_pin.Delete();
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
