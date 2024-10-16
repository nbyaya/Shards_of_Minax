using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
	public class BleedAttackScroll : CSpellScroll
	{
		[Constructable]
		public BleedAttackScroll() : this( 1 )
		{
		}

		[Constructable]
		public BleedAttackScroll( int amount ) : base( typeof( BleedAttackSpell ), 0xE39, amount )
		{
			Name = "Bleed Attack";
			Hue = 1174;
		}

		public BleedAttackScroll( Serial serial ) : base( serial )
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
