using System;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
	public class CartographySpellbookGump : CSpellbookGump
	{
		public override string TextHue  { get{ return "CC3333"; } }
		public override int    BGImage  { get{ return 2203; } }
		public override int    SpellBtn { get{ return 2362; } }
		public override int    SpellBtnP{ get{ return 2361; } }
		public override string Label1   { get{ return "Cartographers Atlas"; } }
		public override string Label2   { get{ return "Spells"; } }
		public override Type   GumpType { get{ return typeof( CartographySpellbookGump ); } }

		public CartographySpellbookGump( CSpellbook book ) : base( book )
		{
		}
	}
}