using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
	public class CrushingBlowScroll : CSpellScroll
	{
		[Constructable]
		public CrushingBlowScroll() : this( 1 )
		{
		}

		[Constructable]
		public CrushingBlowScroll( int amount ) : base( typeof( CrushingBlowSpell ), 0xE39, amount )
		{
			Name = "Crushing Blow";
			Hue = 1174;
		}

		public CrushingBlowScroll( Serial serial ) : base( serial )
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