using System;
using Server;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class SwordsMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallAllies), "Call Allies", "Call Allies to defend you", null, "Mana: 25", 21005, 9301, School.SwordmastersCodex);
			Register(typeof(Cleave), "Cleave", "Unleashes a powerful swing that damages multiple enemies in a wide arc.", null, "Mana: 20", 21001, 9400, School.SwordmastersCodex);
            Register(typeof(Impale), "Impale", "Delivers a piercing thrust that bypasses armor.", null, "Mana: 25", 21002, 9401, School.SwordmastersCodex);
            Register(typeof(WhirlwindStrike), "Whirlwind Strike", "Spins rapidly to strike all nearby enemies.", null, "Mana: 30", 21003, 9402, School.SwordmastersCodex);
            Register(typeof(Riposte), "Riposte", "A quick counterattack following a successful block.", null, "Mana: 15", 21004, 9403, School.SwordmastersCodex);
            Register(typeof(DecisiveStrike), "Decisive Strike", "A focused attack aimed at exploiting the target's weak points.", null, "Mana: 35", 21005, 9404, School.SwordmastersCodex);
            Register(typeof(ArmorPiercer), "Armor Piercer", "A specialized attack designed to penetrate armor.", null, "Mana: 20", 21006, 9405, School.SwordmastersCodex);
            Register(typeof(EnsnaringBlow), "Ensnaring Blow", "A powerful strike that temporarily entangles the target.", null, "Mana: 25", 21007, 9406, School.SwordmastersCodex);
            Register(typeof(ParryMastery), "Parry Mastery", "Enhances the user's ability to block incoming attacks.", null, "Mana: 20", 21008, 9407, School.SwordmastersCodex);
            Register(typeof(ShieldBreaker), "Shield Breaker", "Targets an opponent's shield to weaken or destroy it.", null, "Mana: 25", 21010, 9409, School.SwordmastersCodex);
            Register(typeof(QuickReflexes), "Quick Reflexes", "Enhances the user's reaction time and agility.", null, "Mana: 15", 21011, 9410, School.SwordmastersCodex);
            Register(typeof(BattleCry), "Battle Cry", "Lets out a fierce shout that bolsters allies and intimidates foes.", null, "Mana: 20", 21012, 9411, School.SwordmastersCodex);
            Register(typeof(ReinforcedStance), "Reinforced Stance", "Assumes a defensive posture to withstand heavy attacks.", null, "Mana: 30", 21013, 9412, School.SwordmastersCodex);
            Register(typeof(TacticalRetreat), "Tactical Retreat", "Allows the user to quickly disengage from combat.", null, "Mana: 25", 21014, 9413, School.SwordmastersCodex);
            Register(typeof(MightyLeap), "Mighty Leap", "Executes a powerful leap to close the distance with an enemy or evade an attack.", null, "Mana: 20", 21015, 9414, School.SwordmastersCodex);
            Register(typeof(WeaponMastersInsight), "Weapon Masters Insight", "Provides the user with tactical advice on enemy weaknesses.", null, "Mana: 35", 21016, 9415, School.SwordmastersCodex);
        }
    }
}
