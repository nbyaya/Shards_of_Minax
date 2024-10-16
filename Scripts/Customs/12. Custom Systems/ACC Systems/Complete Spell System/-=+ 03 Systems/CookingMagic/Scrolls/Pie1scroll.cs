using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.CookingMagic
{
	public class Pie1Scroll : CSpellScroll
	{
		[Constructable]
		public Pie1Scroll() : this( 1 )
		{
		}

		[Constructable]
		public Pie1Scroll( int amount ) : base( typeof( Pie1 ), 0xE39, amount )
		{
			Name = "Pie 1";
			Hue = 1174;
		}

		public Pie1Scroll( Serial serial ) : base( serial )
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
