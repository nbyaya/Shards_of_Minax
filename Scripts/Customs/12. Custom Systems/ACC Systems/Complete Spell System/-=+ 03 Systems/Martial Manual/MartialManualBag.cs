using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
	public class ManualBag : ScrollBag
	{
		[Constructable]
		public ManualBag()
		{
			Hue = 1174;
			PlaceItemIn( 30, 35, new WhirlwindAttackScroll() );
			PlaceItemIn( 50, 35, new BleedAttackScroll() );
			PlaceItemIn( 70, 35, new CrushingBlowScroll() );
		}

		public ManualBag( Serial serial ) : base( serial )
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