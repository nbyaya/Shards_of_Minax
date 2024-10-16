using System;
using Server;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class ForensicsMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(SpectralAnalysis), "Spectral Analysis", "Summon a spectral entity to reveal hidden enemies or traps in a specified area.", null, "Mana: 20", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(CrypticStrike), "Cryptic Strike", "Unleash a powerful attack that deals extra damage to enemies based on their wounds or injuries.", null, "Mana: 25", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(TraumaInfliction), "Trauma Infliction", "Inflict a debilitating condition on the target, reducing their combat effectiveness.", null, "Mana: 30", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(WoundAnalysis), "Wound Analysis", "Apply a focused attack that deals increased damage to enemies suffering from specific types of injuries.", null, "Mana: 20", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(PainAmplifier), "Pain Amplifier", "Amplify the pain of a wounded enemy, causing them to take increased damage from all sources.", null, "Mana: 25", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(TraumaticSurge), "Traumatic Surge", "Trigger a surge of psychic energy that can disorient and confuse enemies within a radius.", null, "Mana: 30", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(EvidenceCollection), "Evidence Collection", "Collect clues from an area to uncover hidden objects or secrets.", null, "Mana: 20", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(WoundDetection), "Wound Detection", "Reveal the health status and type of injuries on a target or ally.", null, "Mana: 25", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(TraceAnalysis), "Trace Analysis", "Follow a trail left by an individual or creature, revealing their path and recent actions.", null, "Mana: 20", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(ForensicHealing), "Forensic Healing", "Heal injuries based on detailed forensic knowledge of the wounds.", null, "Mana: 30", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(AutopsyInsight), "Autopsy Insight", "Gain insight into the cause of death or injury of a deceased target, providing useful information.", null, "Mana: 25", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(Decryption), "Decryption", "Analyze and decode encrypted messages or runes, revealing their hidden contents.", null, "Mana: 20", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(ChemicalAnalysis), "Chemical Analysis", "Analyze substances to determine their properties and effects, useful for identifying poisons or potions.", null, "Mana: 25", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(PathologistsInsight), "Pathologist’s Insight", "Identify the cause of death or trauma of creatures or NPCs, revealing their weaknesses or strengths.", null, "Mana: 25", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(ForensicTrapDisarmament), "Forensic Trap Disarmament", "Disarm traps and detect hidden mechanisms in an area.", null, "Mana: 30", 21004, 9300, School.ForensicScholarsTome);
            Register(typeof(InvestigationAura), "Investigation Aura", "Create an aura that enhances the ability to detect hidden objects and clues within a certain radius.", null, "Mana: 20", 21004, 9300, School.ForensicScholarsTome);
        }
    }
}
