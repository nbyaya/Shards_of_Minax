using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.CookingMagic
{
	public class Pie2Scroll : CSpellScroll
	{
		[Constructable]
		public Pie2Scroll() : this( 1 )
		{
		}

		[Constructable]
		public Pie2Scroll( int amount ) : base( typeof( Pie2 ), 0xE39, amount )
		{
			Name = "Pie 2";
			Hue = 1174;
		}

		public Pie2Scroll( Serial serial ) : base( serial )
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