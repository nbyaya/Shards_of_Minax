using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForagersGuidebook
{
	public class ForagersGuidebookForageReagentsSpellScroll : CSpellScroll
	{
		[Constructable]
		public ForagersGuidebookForageReagentsSpellScroll() : this( 1 )
		{
		}

		[Constructable]
		public ForagersGuidebookForageReagentsSpellScroll( int amount ) : base( typeof( ForagersGuidebookForageReagentsSpell ), 0xE39, amount )
		{
			Name = "Forage Reagents";
			Hue = 1174;
		}

		public ForagersGuidebookForageReagentsSpellScroll( Serial serial ) : base( serial )
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
