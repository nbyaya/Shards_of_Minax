using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class SouthernBrand : Item
	{
		[Constructable]
		public SouthernBrand() : base(0x1F23)
		{
			Name = "Southern Brand";
			Hue  = 0x455;
			Weight = 1.0;
		}

		public SouthernBrand(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Brands a map with the Southern Lands facet");
        }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}
			from.SendMessage("Target the map whose realm you wish to brand.");
			from.Target = new BrandTarget(this);
		}

		private class BrandTarget : Target
		{
			private readonly SouthernBrand _brand;
			public BrandTarget(SouthernBrand brand) : base(12, false, TargetFlags.None)
			{
				_brand = brand;
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				if (_brand.Deleted) return;

				if (targ is MagicMapBase map && map.IsChildOf(from.Backpack))
				{
					map.SetDestinationFacet(Map.Map10);
					from.SendMessage("The map now resonates with Southern energy.");
					_brand.Delete();
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
