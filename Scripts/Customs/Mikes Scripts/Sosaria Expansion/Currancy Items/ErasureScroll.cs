using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class ErasureScroll : Item
	{
		[Constructable]
		public ErasureScroll() : base(0x0E34)
		{
			Name = "Scroll of Erasure";
			Hue  = 0x483;
			Weight = 0.5;
		}

		public ErasureScroll(Serial s) : base(s) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}
			from.SendMessage("Target the map whose locations you wish to erase.");
			from.Target = new EraseTarget(this);
		}

		private class EraseTarget : Target
		{
			private readonly ErasureScroll _scroll;
			public EraseTarget(ErasureScroll scroll) : base(12, false, TargetFlags.None)
			{
				_scroll = scroll;
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				if (_scroll.Deleted) return;
				if (targ is MagicMapBase map && map.IsChildOf(from.Backpack))
				{
					map.ClearPredefinedLocations();
					from.SendMessage("All predefined locations have faded from the parchment.");
					_scroll.Delete();
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
