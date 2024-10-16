using System;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.StealingMagic
{
	public class StealingSpellbook : CSpellbook
	{
		public override School School{ get{ return School.RoguesCompendium; } }

		[Constructable]
		public StealingSpellbook() : this( (ulong)0, CSSettings.FullSpellbooks )
		{
		}

		[Constructable]
		public StealingSpellbook( bool full ) : this( (ulong)0, full )
		{
		}

		[Constructable]
		public StealingSpellbook( ulong content, bool full ) : base( content, 0xEFA, full )
		{
			Hue = 2230;
			Name = "Rogues Compendium";
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

			from.CloseGump( typeof( StealingSpellbookGump ) );
			from.SendGump( new StealingSpellbookGump( this ) );
		}

		public StealingSpellbook( Serial serial ) : base( serial )
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