using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.CookingMagic
{
	public class Pie3Scroll : CSpellScroll
	{
		[Constructable]
		public Pie3Scroll() : this( 1 )
		{
		}

		[Constructable]
		public Pie3Scroll( int amount ) : base( typeof( Pie3 ), 0xE39, amount )
		{
			Name = "Pie 3";
			Hue = 1174;
		}

		public Pie3Scroll( Serial serial ) : base( serial )
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
