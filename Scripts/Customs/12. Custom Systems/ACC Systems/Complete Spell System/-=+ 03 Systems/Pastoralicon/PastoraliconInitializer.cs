using System;
using Server;

namespace Server.ACC.CSS.Systems.Pastoralicon
{
	public class PastoraliconInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register( typeof( SheepSpell ),   "Sheep Herd",  "Summon mightly battle sheep", null, "Mana: 45", 2279, 9300, School.Pastoralicon );
			Register( typeof( WolfSpell ),    "Wolf Herd",   "Summon powerful wolves", null, "Mana: 40", 2279, 9300, School.Pastoralicon );
			Register( typeof( SnakeSpell ), "Snake Herd", "Summon poison snakes", null, "Mana: 50", 2279, 9300, School.Pastoralicon );
			Register( typeof( ApeSpell ), "Ape Herd", "Summon a herd of apes", null, "Mana: 45", 2279, 9300, School.Pastoralicon );
			Register( typeof( BearSpell ), "Bear Herd", "Summon a herd of bears", null, "Mana: 55", 2279, 9300, School.Pastoralicon );
			Register( typeof( BirdSpell ), "Bird Herd", "Summon a herd of birds", null, "Mana: 30", 2279, 9300, School.Pastoralicon );
			Register( typeof( CatSpell ), "Cat Herd", "Summon a herd of cats", null, "Mana: 39", 2279, 9300, School.Pastoralicon );
			Register( typeof( DeerSpell ), "Deer Herd", "Summon a herd of deer", null, "Mana: 35", 2279, 9300, School.Pastoralicon );
			Register( typeof( FarmSpell ), "Farm Herd", "Summon a herd of farm animals", null, "Mana: 15", 2279, 9300, School.Pastoralicon );
			Register( typeof( RodentSpell ), "Rodent Herd", "Summon a herd of rodents", null, "Mana: 10", 2279, 9300, School.Pastoralicon );
			Register( typeof( RuminantSpell ), "Ruminant Herd", "Summon a herd of ruminants", null, "Mana: 37", 2279, 9300, School.Pastoralicon );
			Register( typeof( SwampSpell ), "Swamp Herd", "Summon a herd of swamp creatures", null, "Mana: 15", 2279, 9300, School.Pastoralicon );
			Register( typeof( DesertSpell ), "Desert Herd", "Summon a herd of Desert creatures", null, "Mana: 28", 2279, 9300, School.Pastoralicon );
		}
	}
}