using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForagersGuidebook
{
	public class CookingBag : ScrollBag
	{
		[Constructable]
		public CookingBag()
		{
			Hue = 1174;
			PlaceItemIn( 30, 35, new ForagersGuidebookForageReagentsSpellScroll() );
			PlaceItemIn( 50, 35, new ForageToolsSpellScroll() );
			PlaceItemIn( 70, 35, new ForagersForagePlantsSpellScroll() );
		}

		public CookingBag( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}