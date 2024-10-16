using System;
using Server;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class NinjitsuMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallNinjas), "Call Ninjas", "Call Ninjas to defend you", null, "Mana: 25", 21005, 9301, School.ShadowWarriorsManual);
			Register(typeof(ShadowStrike), "Shadow Strike", "A quick, stealthy attack that deals bonus damage if performed from stealth. Chance to cause a bleed effect.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(SmokeBomb), "Smoke Bomb", "Creates a cloud of smoke that obscures vision, reducing the accuracy of enemies within its area. Can be used to escape or gain a tactical advantage.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(ShurikenToss), "Shuriken Toss", "Throws a set of shurikens at the target, dealing ranged damage and causing a disorienting effect that reduces the target's attack speed.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(ShadowCloneSpell), "Shadow Clone", "Creates a temporary illusion of the ninja that mimics their actions. The clone can distract enemies and absorb a portion of incoming damage.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(BlindingFlash), "Blinding Flash", "Emits a blinding flash of light that temporarily blinds enemies, reducing their hit chance and making them vulnerable to follow-up attacks.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(NinjasFury), "Ninjas Fury", "Unleashes a flurry of fast strikes in a short period, dealing multiple hits to a single target. Each hit has a chance to stun the target.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(SilentBlade), "Silent Blade", "An assassination move that delivers a powerful strike with increased damage if the target is unaware of the ninja's presence.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(DeathBlossom), "Death Blossom", "Performs a spinning attack with a wide area of effect, damaging all nearby enemies and applying a bleeding effect.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(ShadowWalk), "Shadow Walk", "Allows the ninja to move through shadows, becoming nearly invisible and passing through obstacles for a short duration.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(NinjasReflexes), "Ninjas Reflexes", "Grants a temporary boost to speed and reflexes, allowing the ninja to dodge attacks and move more swiftly.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(EscapeArtist), "Escape Artist", "Removes all crowd control effects and provides a burst of speed to quickly escape dangerous situations.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(Camouflage), "Camouflage", "Allows the ninja to blend into their surroundings, reducing their visibility and making them harder to detect.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(TrapMastery), "Trap Mastery", "Enables the ninja to set traps that can immobilize or damage enemies who trigger them. Traps can be hidden in various environments.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(HealingSalve), "Healing Salve", "Applies a special salve that heals wounds and removes minor debuffs. Can be used on self or allies.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
            Register(typeof(ShadowMeld), "Shadow Meld", "Temporarily merges with the shadows, becoming untargetable and regaining a portion of health. Useful for evading powerful attacks.", null, "Mana: 20", 21004, 9300, School.ShadowWarriorsManual);
        }
    }
}
