using System;

namespace Server.ACC.CSS.Systems.StealthMagic
{
	public class StealthSpellbookGump : CSpellbookGump
	{
		public override string TextHue  { get{ return "CC3333"; } }
		public override int    BGImage  { get{ return 2203; } }
		public override int    SpellBtn { get{ return 2362; } }
		public override int    SpellBtnP{ get{ return 2361; } }
		public override string Label1   { get{ return "Silent Shadows Journal"; } }
		public override string Label2   { get{ return "Spells"; } }
		public override Type   GumpType { get{ return typeof( StealthSpellbookGump ); } }

		public StealthSpellbookGump( CSpellbook book ) : base( book )
		{
		}
	}
}