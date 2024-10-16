using System;
using Server;

namespace Server.ACC.CSS.Systems.ForagersGuidebook
{
	public class ForagersGuidebookInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register( typeof( ForagersGuidebookForageReagentsSpell ),   "Forage Reagents",  "Search the area for reagents",      null, "Mana: 10", 2248, 9300, School.ForagersGuidebook );
			Register( typeof( ForageToolsSpell ),    "Forage Tools",   "Search the area for tools", null, "Mana: 10", 2248, 9300, School.ForagersGuidebook );
			Register( typeof( ForagersForagePlantsSpell ), "Forage Plants", "Search the area for plants",                                       null, "Mana: 10", 2248, 9300, School.ForagersGuidebook );
		}
	}
}