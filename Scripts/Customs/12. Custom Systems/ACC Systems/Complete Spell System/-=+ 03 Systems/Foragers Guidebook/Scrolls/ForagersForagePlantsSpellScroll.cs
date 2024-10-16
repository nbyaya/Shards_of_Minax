using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForagersGuidebook
{
	public class ForagersForagePlantsSpellScroll : CSpellScroll
	{
		[Constructable]
		public ForagersForagePlantsSpellScroll() : this( 1 )
		{
		}

		[Constructable]
		public ForagersForagePlantsSpellScroll( int amount ) : base( typeof( ForagersForagePlantsSpell ), 0xE39, amount )
		{
			Name = "Forage Plants";
			Hue = 1174;
		}

		public ForagersForagePlantsSpellScroll( Serial serial ) : base( serial )
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