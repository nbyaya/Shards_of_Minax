using System;
using Server;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class HidingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallAllies), "Call Allies", "Call Allies to defend you", null, "Mana: 25", 21005, 9301, School.ShadowsManual);
			Register(typeof(ShadowStep), "Shadow Step", "Quickly teleport a short distance while remaining invisible. Useful for repositioning or escaping.", null, "Mana: 10", 21001, 9200, School.ShadowsManual);
            Register(typeof(VeilOfShadows), "Veil of Shadows", "Create a temporary area of darkness that obscures vision for enemies within it, making it harder for them to spot you.", null, "Mana: 20", 21002, 9201, School.ShadowsManual);
            Register(typeof(SilentFootfalls), "Silent Footfalls", "Move silently for a short duration, making it difficult for enemies to hear your approach or detect your presence.", null, "Mana: 15", 21003, 9202, School.ShadowsManual);
            Register(typeof(DisappearingAct), "Disappearing Act", "Instantly become invisible for a short period, allowing for a quick escape or ambush.", null, "Mana: 25", 21004, 9203, School.ShadowsManual);
            Register(typeof(CloakOfTheNight), "Cloak of the Night", "Increase your hiding skill for a period, improving your chances of remaining unseen by enemies.", null, "Mana: 20", 21005, 9204, School.ShadowsManual);
            Register(typeof(Camouflage), "Camouflage", "Blend into your surroundings, making it harder for enemies to spot you while you remain stationary.", null, "Mana: 20", 21006, 9205, School.ShadowsManual);
            Register(typeof(GhostWalk), "Ghost Walk", "Become incorporeal for a few seconds, allowing you to pass through obstacles and avoid physical attacks.", null, "Mana: 30", 21007, 9206, School.ShadowsManual);
            Register(typeof(SneakAttack), "Sneak Attack", "Perform a surprise attack that deals extra damage if you are hidden or have been invisible.", null, "Mana: 25", 21008, 9207, School.ShadowsManual);
            Register(typeof(Disguise), "Disguise", "Temporarily change your appearance to blend in with a specific group or environment, making it harder for enemies to recognize you.", null, "Mana: 30", 21009, 9208, School.ShadowsManual);
            Register(typeof(EscapeArtist), "Escape Artist", "Increase your ability to break free from traps or restraints, enhancing your chances of evading capture.", null, "Mana: 15", 21010, 9209, School.ShadowsManual);
            Register(typeof(IllusionaryDoubleSpell), "Illusionary Double", "Create a temporary illusion of yourself that distracts enemies and draws their attention away from your actual location.", null, "Mana: 25", 21011, 9210, School.ShadowsManual);
            Register(typeof(SilentStrike), "Silent Strike", "Perform a melee attack with increased damage while remaining silent, preventing enemies from detecting your position.", null, "Mana: 25", 21013, 9212, School.ShadowsManual);
            Register(typeof(ShadowBind), "Shadow Bind", "Temporarily bind an enemy in place with shadowy tendrils, preventing them from moving or attacking.", null, "Mana: 30", 21014, 9213, School.ShadowsManual);
            Register(typeof(FeignDeath), "Feign Death", "Pretend to be dead for a short duration, causing enemies to ignore you until you move or attack.", null, "Mana: 15", 21015, 9214, School.ShadowsManual);
            Register(typeof(WhisperingWinds), "Whispering Winds", "Send a message or signal to an ally while remaining hidden, allowing for discreet communication during combat or exploration.", null, "Mana: 15", 21016, 9215, School.ShadowsManual);
        }
    }
}
