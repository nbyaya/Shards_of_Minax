using System;
using Server;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class MusicianshipMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(SummonAllies), "Summon Allies", "Summon Allies to defend you", null, "Mana: 25", 21005, 9301, School.VirtuososFolio);
			Register(typeof(HarmoniousStrike), "Harmonious Strike", "Unleash a powerful musical note that deals damage and briefly stuns your target.", null, "Mana: 20", 21004, 9300, School.VirtuososFolio);
            Register(typeof(ResonantWail), "Resonant Wail", "Emit a haunting melody that causes enemies within range to flee in fear for a short duration.", null, "Mana: 25", 21004, 9300, School.VirtuososFolio);
            Register(typeof(CadenceOfConfusion), "Cadence of Confusion", "Play a disorienting tune that confuses enemies, causing them to attack their allies or miss their attacks.", null, "Mana: 30", 21004, 9300, School.VirtuososFolio);
            Register(typeof(EtherealMelody), "Ethereal Melody", "Summon a spectral entity that attacks enemies with musical energy, dealing damage over time.", null, "Mana: 35", 21004, 9300, School.VirtuososFolio);
            Register(typeof(SymphonicBurst), "Symphonic Burst", "Create a burst of sound that deals area-of-effect damage to all enemies within a certain radius.", null, "Mana: 40", 21004, 9300, School.VirtuososFolio);
            Register(typeof(SoothingSerenade), "Soothing Serenade", "Play a calming melody that heals and removes minor debuffs from allies.", null, "Mana: 50", 21004, 9300, School.VirtuososFolio);
            Register(typeof(EchoesOfRebirth), "Echoes of Rebirth", "A song that revives a fallen ally with a portion of their health restored.", null, "Mana: 55", 21004, 9300, School.VirtuososFolio);
            Register(typeof(LullabyOfSleep), "Lullaby of Sleep", "Play a gentle tune that puts enemies to sleep for a short duration, rendering them vulnerable.", null, "Mana: 60", 21004, 9300, School.VirtuososFolio);
            Register(typeof(BardicInspiration), "Bardic Inspiration", "Provide a temporary boost to an ally's skills or attributes, enhancing their performance in combat or other activities.", null, "Mana: 65", 21004, 9300, School.VirtuososFolio);
            Register(typeof(MelodicRecall), "Melodic Recall", "Instantly teleport to a previously marked location, useful for quick escapes or returning to a safe spot.", null, "Mana: 70", 21004, 9300, School.VirtuososFolio);
            Register(typeof(HarmonyOfRestoration), "Harmony of Restoration", "Restore mana or stamina to yourself or an ally through a revitalizing melody.", null, "Mana: 75", 21004, 9300, School.VirtuososFolio);
            Register(typeof(ShieldOfSound), "Shield of Sound", "Generate a protective barrier of sound waves that absorbs a portion of incoming damage for a limited time.", null, "Mana: 80", 21004, 9300, School.VirtuososFolio);
            Register(typeof(ResonantGuard), "Resonant Guard", "Use a musical shield to deflect incoming attacks and projectiles for a brief period.", null, "Mana: 85", 21004, 9300, School.VirtuososFolio);
            Register(typeof(DissonantShield), "Dissonant Shield", "Create a field of discordant sound that causes damage to enemies who come into close proximity.", null, "Mana: 90", 21004, 9300, School.VirtuososFolio);
            Register(typeof(EchoingRetreat), "Echoing Retreat", "Perform a quick retreat while leaving behind a musical decoy that distracts enemies and absorbs some damage.", null, "Mana: 95", 21004, 9300, School.VirtuososFolio);
        }
    }
}
