using System;
using Server;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class ArmsLoreMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register( typeof( PrecisionStrike ), "Precision Strike", "Temporarily boosts weapon accuracy, increasing hit chance.", null, "Mana: 20", 21004, 9300, School.ArmorersManual );
            Register( typeof( WeaponMastery ), "Weapon Mastery", "Increases damage with a specific weapon type for a short duration.", null, "Mana: 25", 21004, 9300, School.ArmorersManual );
            Register( typeof( Counterattack ), "Counterattack", "Grants a chance to counterattack when blocked or parried.", null, "Mana: 20", 21004, 9300, School.ArmorersManual );
            Register( typeof( PerfectParry ), "Perfect Parry", "Increases the chance of successful parries for a short time.", null, "Mana: 20", 21004, 9300, School.ArmorersManual );
            Register( typeof( WeaponEnhancement ), "Weapon Enhancement", "Temporarily adds bonus damage to your weapon.", null, "Mana: 25", 21004, 9300, School.ArmorersManual );
            Register( typeof( ArmorWeaving ), "Armor Weaving", "Improves the effectiveness of worn armor, reducing damage taken.", null, "Mana: 25", 21004, 9300, School.ArmorersManual );
            Register( typeof( QuickRepair ), "Quick Repair", "Instantly repairs a weapon or piece of armor in your inventory.", null, "Mana: 30", 21004, 9300, School.ArmorersManual );
            Register( typeof( BlindingStrike ), "Blinding Strike", "Temporarily blinds your target, reducing their chance to hit.", null, "Mana: 25", 21004, 9300, School.ArmorersManual );
            Register( typeof( Disarm ), "Disarm", "Chance to disarm your opponent, causing them to drop their weapon.", null, "Mana: 30", 21004, 9300, School.ArmorersManual );
            Register( typeof( BattleFocus ), "Battle Focus", "Increases your ability to detect and exploit weaknesses in enemy armor.", null, "Mana: 20", 21004, 9300, School.ArmorersManual );
            Register( typeof( DefensiveStance ), "Defensive Stance", "Reduces incoming damage by adopting a defensive posture.", null, "Mana: 25", 21004, 9300, School.ArmorersManual );
            Register( typeof( CriticalHit ), "Critical Hit", "Temporarily increases the chance of landing a critical hit with weapons.", null, "Mana: 25", 21004, 9300, School.ArmorersManual );
            Register( typeof( WeaponFortification ), "Weapon Fortification", "Temporarily increases the durability of your weapon, reducing the rate of wear.", null, "Mana: 20", 21004, 9300, School.ArmorersManual );
            Register( typeof( ArmorShatteringStrike ), "Armor Shattering Strike", "Temporarily reduces the effectiveness of your opponent’s armor.", null, "Mana: 30", 21004, 9300, School.ArmorersManual );
            Register( typeof( RapidAssault ), "Rapid Assault", "Increases attack speed for a short duration.", null, "Mana: 25", 21004, 9300, School.ArmorersManual );
            Register( typeof( VeteransInsight ), "Veteran’s Insight", "Provides a temporary boost to Arms Lore skill, enhancing overall combat effectiveness.", null, "Mana: 30", 21004, 9300, School.ArmorersManual );
        }
    }
}
