using System;
using Server;

namespace Server.ACC.CSS.Systems.CookingMagic
{
	public class CookingMagicInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register( typeof( Pie1 ),   "Flavor Burst",  "Enhances the taste of food with a burst of enchanting flavors.",      null, "Tithe: 30; Skill: 80; Mana: 40", 2258, 9300, School.CookingMagic );
			Register( typeof( Pie2 ),    "Culinary Conjure",   "Summons ingredients or cooking utensils with magical prowess.", null, "Tithe: 10; Skill: 20; Mana: 10", 2269, 9300, School.CookingMagic );
			Register( typeof( Pie3 ), "Gastronomic Glamour", "Transforms ordinary dishes into visually stunning and enticing culinary creations .",                                       null, "Tithe: 10; Skill: 20; Mana: 10", 2245, 9300, School.CookingMagic );
		}
	}
}