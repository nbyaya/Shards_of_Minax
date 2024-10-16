using System;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
	public class ChivalrySpellbook2 : CSpellbook
	{
		public override School School{ get{ return School.PaladinsTestament; } }

		[Constructable]
		public ChivalrySpellbook2() : this( (ulong)0, CSSettings.FullSpellbooks )
		{
		}

		[Constructable]
		public ChivalrySpellbook2( bool full ) : this( (ulong)0, full )
		{
		}

		[Constructable]
		public ChivalrySpellbook2( ulong content, bool full ) : base( content, 0xEFA, full )
		{
			Hue = 2110;
			Name = "Paladins Testament";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel == AccessLevel.Player )
			{
				//Container pack = from.Backpack;
				//if( !(Parent == from || (pack != null && Parent == pack)) )
				//{
					//from.SendMessage( "The spellbook must be in your backpack [and not in a container within] to open." );
					//return;
				//}
				//else
				if( SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions( from, this.School ) )
				{
					return;
				}
			}

			from.CloseGump( typeof( ChivalrySpellbook2Gump ) );
			from.SendGump( new ChivalrySpellbook2Gump( this ) );
		}

		public ChivalrySpellbook2( Serial serial ) : base( serial )
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