using System;
using Server;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class DiscordanceMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(DiscordantSurge), "Discordant Surge", "Temporarily boosts the damage output of all allies while causing confusion to enemies.", null, "Mana: 20", 21004, 9300, School.DiscordantCodex);
            Register(typeof(ChaoticResonance), "Chaotic Resonance", "Creates a wave of discord that disorients and damages enemies in a small radius.", null, "Mana: 25", 21004, 9300, School.DiscordantCodex);
            Register(typeof(FrenziedHarmony), "Frenzied Harmony", "Induces a state of rage in a target, increasing their damage output but lowering their defense.", null, "Mana: 20", 21004, 9300, School.DiscordantCodex);
            Register(typeof(DiscordantEcho), "Discordant Echo", "Releases a discordant sound that reflects a portion of incoming damage back to the attacker.", null, "Mana: 25", 21004, 9300, School.DiscordantCodex);
            Register(typeof(TurbulentWail), "Turbulent Wail", "A powerful scream that causes enemies to flee in panic for a short duration.", null, "Mana: 30", 21004, 9300, School.DiscordantCodex);
            Register(typeof(MelodicDespair), "Melodic Despair", "Plays a haunting melody that reduces the morale of enemies, lowering their combat effectiveness.", null, "Mana: 20", 21004, 9300, School.DiscordantCodex);
            Register(typeof(DissonantShout), "Dissonant Shout", "Emits a discordant sound that interrupts enemy spellcasting or abilities.", null, "Mana: 15", 21004, 9300, School.DiscordantCodex);
            Register(typeof(DisruptiveChant), "Disruptive Chant", "A chant that targets an enemy, causing their attacks to become erratic and less effective.", null, "Mana: 20", 21004, 9300, School.DiscordantCodex);
            Register(typeof(ConfoundingAura), "Confounding Aura", "Generates an aura around the user that causes confusion in nearby enemies.", null, "Mana: 25", 21004, 9300, School.DiscordantCodex);
            Register(typeof(DiscordantVeil), "Discordant Veil", "Creates a protective barrier that distorts incoming attacks, reducing damage taken.", null, "Mana: 30", 21004, 9300, School.DiscordantCodex);
            Register(typeof(EchoingSpheres), "Echoing Spheres", "Summons floating spheres that create chaotic energy fields, providing various debuffs to enemies.", null, "Mana: 25", 21004, 9300, School.DiscordantCodex);
            Register(typeof(HarmonicConfusion), "Harmonic Confusion", "Plays a complex tune that muddles the perception of enemies, making it harder for them to distinguish between friend and foe.", null, "Mana: 20", 21004, 9300, School.DiscordantCodex);
            Register(typeof(DisruptiveResonance), "Disruptive Resonance", "Sends out resonant waves that interfere with enemy spellcasting or ability use, reducing their effectiveness.", null, "Mana: 25", 21004, 9300, School.DiscordantCodex);
            Register(typeof(DiscordantEchoes), "Discordant Echoes", "Sends out echoes of previous discordant effects, extending their duration or increasing their impact.", null, "Mana: 30", 21004, 9300, School.DiscordantCodex);
            Register(typeof(ChaoticBeacon), "Chaotic Beacon", "Marks a target with a beacon that causes all allies to deal extra damage to the marked target.", null, "Mana: 20", 21004, 9300, School.DiscordantCodex);
            Register(typeof(EnigmaticMelody), "Enigmatic Melody", "Plays a soothing yet confusing melody that restores a small amount of health to allies while causing enemies to be less effective in combat.", null, "Mana: 25", 21004, 9300, School.DiscordantCodex);
        }
    }
}
