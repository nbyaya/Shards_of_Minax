using System;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
	public class NecromancySpellbookGump : CSpellbookGump
	{
		public override string TextHue  { get{ return "CC3333"; } }
		public override int    BGImage  { get{ return 2203; } }
		public override int    SpellBtn { get{ return 2362; } }
		public override int    SpellBtnP{ get{ return 2361; } }
		public override string Label1   { get{ return "Necromancy Magic"; } }
		public override string Label2   { get{ return "Spells"; } }
		public override Type   GumpType { get{ return typeof( NecromancySpellbookGump ); } }

		public NecromancySpellbookGump( CSpellbook book ) : base( book )
		{
		}
	}
}