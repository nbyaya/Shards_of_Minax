using System;
using Server;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class StealthMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallAllies), "Call Allies", "Call Allies to defend you", null, "Mana: 25", 21005, 9301, School.SilentShadowsJournal);
			Register(typeof(ShadowMeld), "Shadow Meld", "Temporarily become invisible, allowing you to evade detection.", null, "Duration: 10 seconds, Cooldown: 30 seconds", 21001, 9200, School.SilentShadowsJournal);
            Register(typeof(SilentMovement), "Silent Movement", "Reduce the noise made while moving, making it harder for enemies to detect your presence.", null, "Duration: 1 minute, Cooldown: 15 seconds", 21002, 9201, School.SilentShadowsJournal);
            Register(typeof(CloakOfShadows), "Cloak of Shadows", "Create a field of darkness around yourself that reduces visibility for enemies.", null, "Duration: 30 seconds, Cooldown: 1 minute", 21003, 9202, School.SilentShadowsJournal);
            Register(typeof(DisguiseSelf), "Disguise Self", "Temporarily alter your appearance to blend in with your surroundings or mimic someone else.", null, "Duration: 5 minutes, Cooldown: 5 minutes", 21004, 9203, School.SilentShadowsJournal);
            Register(typeof(Snoop), "Snoop", "Eavesdrop on nearby conversations or detect hidden traps and objects within a certain radius.", null, "Duration: 10 seconds, Cooldown: 30 seconds", 21005, 9204, School.SilentShadowsJournal);
            Register(typeof(QuickEscape), "Quick Escape", "Instantly teleport a short distance away to escape danger.", null, "Range: 10 tiles, Cooldown: 1 minute", 21006, 9205, School.SilentShadowsJournal);
            Register(typeof(TrapDetection), "Trap Detection", "Reveal and disable traps in your vicinity.", null, "Duration: 10 seconds, Cooldown: 1 minute", 21007, 9206, School.SilentShadowsJournal);
            Register(typeof(Camouflage), "Camouflage", "Blend into your surroundings, making it difficult for enemies to spot you.", null, "Duration: 1 minute, Cooldown: 1 minute", 21008, 9207, School.SilentShadowsJournal);
            Register(typeof(Backstab), "Backstab", "Perform a surprise attack from stealth that deals extra damage if you are behind your target.", null, "Damage: +50%, Cooldown: 20 seconds", 21009, 9208, School.SilentShadowsJournal);
            Register(typeof(Ambush), "Ambush", "Launch a powerful attack from stealth that incapacitates your target for a short period.", null, "Duration: 5 seconds stun, Cooldown: 30 seconds", 21010, 9209, School.SilentShadowsJournal);
            Register(typeof(SneakAttack), "Sneak Attack", "Deal a critical hit from stealth with a higher chance of bypassing armor.", null, "Damage: +25%, Cooldown: 15 seconds", 21011, 9210, School.SilentShadowsJournal);
            Register(typeof(SmokeBomb), "Smoke Bomb", "Throw a smoke bomb to create a cloud of smoke, obscuring vision and reducing accuracy for enemies within the area.", null, "Duration: 15 seconds, Cooldown: 1 minute", 21013, 9212, School.SilentShadowsJournal);
            Register(typeof(SilentKiller), "Silent Killer", "Perform a silent takedown on a single target, instantly eliminating them if they are unaware of your presence.", null, "Requires target to be below 25% health, Cooldown: 5 minutes", 21014, 9213, School.SilentShadowsJournal);
            Register(typeof(ShadowStep), "Shadow Step", "Teleport behind your target and perform an attack, causing confusion and disorientation.", null, "Duration: 5 seconds disorientation, Cooldown: 45 seconds", 21015, 9214, School.SilentShadowsJournal);
            Register(typeof(VanishingBlow), "Vanishing Blow", "Deal a powerful attack that causes you to immediately become invisible and move away from your target.", null, "Damage: +40%, Cooldown: 1 minute", 21016, 9215, School.SilentShadowsJournal);
        }
    }
}
