using System;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
	public class AnimalLoreSpellbook : CSpellbook
	{
		public override School School{ get{ return School.BestiaryTome; } }

		[Constructable]
		public AnimalLoreSpellbook() : this( (ulong)0, CSSettings.FullSpellbooks )
		{
		}

		[Constructable]
		public AnimalLoreSpellbook( bool full ) : this( (ulong)0, full )
		{
		}

		[Constructable]
		public AnimalLoreSpellbook( ulong content, bool full ) : base( content, 0xEFA, full )
		{
			Hue = 600;
			Name = "Bestiary Tome";
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

			from.CloseGump( typeof( AnimalLoreSpellbookGump ) );
			from.SendGump( new AnimalLoreSpellbookGump( this ) );
		}

		public AnimalLoreSpellbook( Serial serial ) : base( serial )
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