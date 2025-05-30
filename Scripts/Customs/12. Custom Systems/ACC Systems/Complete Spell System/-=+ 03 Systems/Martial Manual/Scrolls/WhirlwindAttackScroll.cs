using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
	public class WhirlwindAttackScroll : CSpellScroll
	{
		[Constructable]
		public WhirlwindAttackScroll() : this( 1 )
		{
		}

		[Constructable]
		public WhirlwindAttackScroll( int amount ) : base( typeof( WhirlwindAttackSpell ), 0xE39, amount )
		{
			Name = "Whirlwind Attack";
			Hue = 1174;
		}

		public WhirlwindAttackScroll( Serial serial ) : base( serial )
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
