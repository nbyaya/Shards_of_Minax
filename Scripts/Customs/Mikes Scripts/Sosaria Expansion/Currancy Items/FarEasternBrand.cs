using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class FarEasternBrand : Item
	{
		[Constructable]
		public FarEasternBrand() : base(0x1F23)
		{
			Name = "FarEastern Brand";
			Hue  = 0x455;
			Weight = 1.0;
		}

		public FarEasternBrand(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Brands a map with the Far Eastern Lands facet");
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
			private readonly FarEasternBrand _brand;
			public BrandTarget(FarEasternBrand brand) : base(12, false, TargetFlags.None)
			{
				_brand = brand;
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				if (_brand.Deleted) return;

				if (targ is MagicMapBase map && map.IsChildOf(from.Backpack))
				{
					map.SetDestinationFacet(Map.Map12);
					from.SendMessage("The map now resonates with Far Eastern energy.");
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
