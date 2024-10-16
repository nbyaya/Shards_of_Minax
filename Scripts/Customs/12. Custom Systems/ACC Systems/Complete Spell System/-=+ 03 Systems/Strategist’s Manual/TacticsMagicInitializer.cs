using System;
using Server;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class TacticsMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallAllies), "Call Allies", "Call Allies to defend you", null, "Mana: 25", 21005, 9301, School.StrategistsManual);
			Register(typeof(PrecisionStrike), "Precision Strike", "Focus your attack for increased damage and a chance to bypass armor.", null, "Mana: 20", 21004, 9300, School.StrategistsManual);
            Register(typeof(Cleave), "Cleave", "Swing your weapon in a wide arc, dealing damage to multiple enemies in front of you.", null, "Mana: 25", 21004, 9300, School.StrategistsManual);
            Register(typeof(SavageBlow), "Savage Blow", "A powerful, focused attack with a chance to stun the target.", null, "Mana: 30", 21004, 9300, School.StrategistsManual);
            Register(typeof(Counterstrike), "Counterstrike", "Immediately counter an incoming attack with a precise strike that deals extra damage.", null, "Mana: 15", 21004, 9300, School.StrategistsManual);
            Register(typeof(WhirlwindAttack), "Whirlwind Attack", "Spin rapidly, attacking all enemies around you with a chance to knock them down.", null, "Mana: 40", 21004, 9300, School.StrategistsManual);
            Register(typeof(PenetratingStrike), "Penetrating Strike", "Deliver a concentrated blow that has a higher chance to penetrate enemy defenses.", null, "Mana: 25", 21004, 9300, School.StrategistsManual);
            Register(typeof(Char), "Charge", "Rush towards an enemy, dealing damage and pushing them back.", null, "Mana: 30", 21004, 9300, School.StrategistsManual);
            Register(typeof(TacticalRetreat), "Tactical Retreat", "Quickly move away from combat, leaving a temporary decoy to distract enemies.", null, "Mana: 20", 21004, 9300, School.StrategistsManual);
            Register(typeof(BattlefieldAwareness), "Battlefield Awareness", "Increase your ability to detect hidden enemies and traps within a certain radius.", null, "Mana: 20", 21004, 9300, School.StrategistsManual);
            Register(typeof(StrategicHeal), "Strategic Heal", "Apply a quick healing spell that restores a portion of health and increases resistances temporarily.", null, "Mana: 30", 21004, 9300, School.StrategistsManual);
            Register(typeof(EnemyWeakness), "Enemy Weakness", "Identify a specific weakness in an enemy, increasing your team's damage against that target.", null, "Mana: 25", 21004, 9300, School.StrategistsManual);
            Register(typeof(InspiringRally), "Inspiring Rally", "Rally your allies with a battle cry, temporarily boosting their attack power and morale.", null, "Mana: 25", 21004, 9300, School.StrategistsManual);
            Register(typeof(DisarmTrap), "Disarm Trap", "Safely neutralize traps or hazards on the battlefield, making it safer for you and your allies.", null, "Mana: 20", 21004, 9300, School.StrategistsManual);
            Register(typeof(TacticalShift), "Tactical Shift", "Instantly reposition yourself to a more advantageous location in the heat of battle.", null, "Mana: 15", 21004, 9300, School.StrategistsManual);
            Register(typeof(CommandingPresence), "Commanding Presence", "Exert control over the battlefield, forcing enemies to focus on you and reducing their effectiveness.", null, "Mana: 30", 21004, 9300, School.StrategistsManual);
        }
    }
}
