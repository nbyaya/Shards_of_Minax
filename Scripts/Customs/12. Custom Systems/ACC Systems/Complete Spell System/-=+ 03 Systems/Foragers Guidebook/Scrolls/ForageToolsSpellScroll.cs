using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForagersGuidebook
{
	public class ForageToolsSpellScroll : CSpellScroll
	{
		[Constructable]
		public ForageToolsSpellScroll() : this( 1 )
		{
		}

		[Constructable]
		public ForageToolsSpellScroll( int amount ) : base( typeof( ForageToolsSpell ), 0xE39, amount )
		{
			Name = "Forage Tools";
			Hue = 1174;
		}

		public ForageToolsSpellScroll( Serial serial ) : base( serial )
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
