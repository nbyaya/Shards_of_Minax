using System;
using Server;

namespace Server.ACC.CSS.Systems.MartialManual
{
	public class MartialManualInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register( typeof( WhirlwindAttackSpell ),   "Whirlwind Attack",  "Attack All Enemies", null, "Mana: 15", 21004, 9300, School.MartialManual );
			Register( typeof( BleedAttackSpell ),    "Bleed Attack",   "Cause Enemy to Bleed", null, "Mana: 30", 20993, 9300, School.MartialManual );
			Register( typeof( CrushingBlowSpell ), "Crushing Blow", "Crush Enemy Defenses", null, "Mana: 20", 20995, 9300, School.MartialManual );
			Register( typeof( RidingSwipeSpell ), "Riding Swipe", "Dismounts your opponent and damage", null, "Mana: 25", 20745, 9300, School.MartialManual );
			Register( typeof( NerveStrikeSpell ), "Nerve Strike", "Paralyses your opponent for a short time", null, "Mana: 30", 20487, 9300, School.MartialManual );
			Register( typeof( MortalStrikeSpell ), "Mortal Strike", "A successful Mortal Strike will render its victim unable to heal", null, "Mana: 30", 23013, 9300, School.MartialManual );
			Register( typeof( InfectiousStrikeSpell ), "Infectious Strike", "Infectious Strike requires a weapon with poison charges on it", null, "Mana: 20", 20489, 9300, School.MartialManual );
			Register( typeof( FeintSpell ), "Feint", "Reduces the attacker’s damage by a percentage", null, "Mana: 30", 21011, 9300, School.MartialManual );
			Register( typeof( DisrobeSpell ), "Disrobe", "Remove enemy clothes", null, "Mana: 25", 21008, 9300, School.MartialManual );
			Register( typeof( DismountSpell ), "Dismount", "Unseat a mounted opponent", null, "Mana: 25", 20997, 9300, School.MartialManual );
			Register( typeof( DisarmSpell ), "Disarm", "This attack allows you to disarm your foe", null, "Mana: 20", 20996, 9300, School.MartialManual );
			Register( typeof( DefenseMasterySpell ), "Defense Mastery", "Raises your physical resistance for a short time", null, "Mana: 20", 20743, 9300, School.MartialManual );
			Register( typeof( ConcussionBlowSpell ), "Concussion Blow", "Crush Enemy Defenses", null, "Mana: 20", 20994, 9300, School.MartialManual );
			Register( typeof( ColdWindSpell ), "Cold Wind", "Creates a cold wind", null, "Mana: 20", 20995, 9300, School.MartialManual );
			Register( typeof( BattleLustSpell ), "Battle Lust", "Gain a lust for battle", null, "Mana: 15", 21006, 9300, School.MartialManual );
			Register( typeof( ArmorIgnoreSpell ), "Armor Ignore", "Bypass all of their target’s armor resistance", null, "Mana: 30", 20995, 9300, School.MartialManual );
		}
	}
}