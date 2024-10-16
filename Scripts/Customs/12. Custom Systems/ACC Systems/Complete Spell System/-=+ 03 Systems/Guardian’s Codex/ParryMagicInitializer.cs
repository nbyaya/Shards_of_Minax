using System;
using Server;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class ParryMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(DeflectiveStance), "Deflective Stance", "Temporarily increases your chance to parry incoming attacks by 25% while reducing your movement speed.", null, "Mana: 10", 21005, 9301, School.GuardiansCodex);
            Register(typeof(PerfectParry), "Perfect Parry", "Grants a high chance to parry a single incoming attack and counter with a quick strike.", null, "Mana: 15", 21005, 9301, School.GuardiansCodex);
            Register(typeof(ShieldBash), "Shield Bash", "Use your shield to stun an opponent for a few seconds, causing them to lose their next attack.", null, "Mana: 20", 21005, 9301, School.GuardiansCodex);
            Register(typeof(Counterstrike), "Counterstrike", "After a successful parry, immediately counter with a powerful melee attack that deals additional damage.", null, "Mana: 20", 21005, 9301, School.GuardiansCodex);
            Register(typeof(AegisOfProtection), "Aegis of Protection", "Creates a temporary shield that absorbs a portion of incoming damage for a short duration.", null, "Mana: 25", 21005, 9301, School.GuardiansCodex);
            Register(typeof(Riposte), "Riposte", "After successfully parrying an attack, perform a quick, precise strike that deals extra damage based on the opponent’s attack strength.", null, "Mana: 20", 21005, 9301, School.GuardiansCodex);
            Register(typeof(GuardedAssault), "Guarded Assault", "Launch a fierce attack that gains increased accuracy and damage if you have successfully parried at least once in the last 5 seconds.", null, "Mana: 25", 21005, 9301, School.GuardiansCodex);
            Register(typeof(ReflectiveShield), "Reflective Shield", "Reflect a portion of the damage back to the attacker when you successfully parry their attack.", null, "Mana: 25", 21005, 9301, School.GuardiansCodex);
            Register(typeof(GuardDuty), "Guard Duty", "Temporarily increase the parry skill of all allies within a certain radius, providing them with better defensive capabilities.", null, "Mana: 30", 21005, 9301, School.GuardiansCodex);
            Register(typeof(FortifiedDefense), "Fortified Defense", "Temporarily boost your parry skill and reduce the chance of being hit critically by incoming attacks.", null, "Mana: 25", 21005, 9301, School.GuardiansCodex);
            Register(typeof(QuickReflexes), "Quick Reflexes", "Increase your reaction speed, granting a brief period where you can parry attacks with greater efficiency.", null, "Mana: 15", 21005, 9301, School.GuardiansCodex);
            Register(typeof(ShieldingAura), "Shielding Aura", "Create an aura around you that reduces damage taken by allies within a certain radius when you successfully parry attacks.", null, "Mana: 30", 21005, 9301, School.GuardiansCodex);
            Register(typeof(EnhancedGuard), "Enhanced Guard", "Provides a temporary buff that reduces the stamina cost of parrying for a short duration.", null, "Mana: 20", 21005, 9301, School.GuardiansCodex);
            Register(typeof(DefensiveFormation), "Defensive Formation", "Activate a defensive stance that reduces incoming damage and increases the effectiveness of parrying for a short period.", null, "Mana: 30", 21005, 9301, School.GuardiansCodex);
            Register(typeof(EmergencyEvasion), "Emergency Evasion", "Instantly dodge and parry the next attack if you are hit below a certain health threshold.", null, "Mana: 30", 21005, 9301, School.GuardiansCodex);
            Register(typeof(ReinforcedShield), "Reinforced Shield", "Temporarily fortify your shield, increasing its durability and effectiveness at absorbing damage.", null, "Mana: 25", 21005, 9301, School.GuardiansCodex);
        }
    }
}
