using System;
using Server;

namespace Server.ACC.CSS.Systems.FishingMagic
{
	public class FishingMagicInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register( typeof( TidalWave ),   "Tidal Wave",  "Unleash a wave of water to knock back and stun nearby enemies.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( FishingSpear ),   "Fishing Spear",  "Summon a magical spear that deals water-based damage to a single target.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( NetTrap ),   "Net Trap",  "Deploy a magical fishing net that ensnares and immobilizes enemies within its area.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( WaterDragonsWrath ),   "Water Dragons Wrath",  "Call forth a water dragon to attack enemies with a powerful water-based assault.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( StormSurge ),   "Storm Surge",  "Create a localized storm that deals electrical damage to enemies and increases the chance of fishing success.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( AquaticShield ),   "Aquatic Shield",  "Form a protective barrier of water around yourself or an ally to absorb incoming damage.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( BaitMastery ),   "Bait Mastery",  "Increase the effectiveness of bait, attracting rare and valuable fish more often.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( UnderwaterBreathing ),   "Underwater Breathing",  "Temporarily allow yourself and allies to breathe underwater, making it easier to fish in deep or submerged areas.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( FishingFocus ),   "Fishing Focus",  "Enhance your fishing skills for a short period, improving the chances of catching high-quality fish.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( FishingRadar ),   "Fishing Radar",  "Reveal the location of nearby fish and underwater resources for easier fishing.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( ReelMastery ),   "Reel Mastery",  "Improve the speed and efficiency of reeling in fish, reducing the time it takes to catch them.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( WaterWalk ),   "Water Walk",  "Temporarily allow yourself and allies to walk on water, making it easier to fish from any location.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( QuickRelease ),   "Quick Release",  "Instantly release a caught fish back into the water if it's not the desired catch, preserving bait and reducing frustration.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( FishingFrenzy ),   "Fishing Frenzy",  "Summon a school of fish to swarm the area, increasing the number of catches and potentially attracting rare fish.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( OceanBlessing ),   "Oceans Blessing",  "Temporarily grant increased fishing skill and luck to yourself or allies, boosting overall fishing effectiveness.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );
			Register( typeof( MysticCatch ),   "Mystic Catch",  "Occasionally catch a mystical fish that provides a temporary buff or unique bonus when consumed.", null, "Mana: 15", 21004, 9300, School.AnglersGuide );			

		}
	}
}