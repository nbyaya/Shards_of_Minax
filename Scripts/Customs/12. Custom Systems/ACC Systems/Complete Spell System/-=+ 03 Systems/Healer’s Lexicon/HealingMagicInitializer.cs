using System;
using Server;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class HealingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(DivineRestoration), "Divine Restoration", "Instantly heal a target and remove harmful debuffs or status effects.", null, "Mana: 20", 21004, 9300, School.HealersLexicon);
            Register(typeof(SanctuaryField), "Sanctuary Field", "Create a protective aura around a target that reduces incoming damage for a short duration.", null, "Mana: 20", 21004, 9300, School.HealersLexicon);
            Register(typeof(RadiantHeal), "Radiant Heal", "Heal a target and deal light damage to nearby enemies based on the healing amount.", null, "Mana: 20", 21004, 9300, School.HealersLexicon);
            Register(typeof(PurifyingLight), "Purifying Light", "A powerful heal that also dispels negative magic effects from the target.", null, "Mana: 20", 21004, 9300, School.HealersLexicon);
            Register(typeof(HealingWave), "Healing Wave", "Heal all allies in a small radius around the caster with a single powerful spell.", null, "Mana: 30", 21004, 9300, School.HealersLexicon);
            Register(typeof(BlessedRevival), "Blessed Revival", "Revive a fallen ally with a portion of their health and mana restored.", null, "Mana: 30", 21004, 9300, School.HealersLexicon);
            Register(typeof(CleansingSurge), "Cleansing Surge", "Quickly heal an ally while removing poisons or diseases affecting them.", null, "Mana: 20", 21004, 9300, School.HealersLexicon);
            Register(typeof(HealingShield), "Healing Shield", "Create a shield around an ally that absorbs a certain amount of incoming damage and converts it into healing.", null, "Mana: 25", 21004, 9300, School.HealersLexicon);
            
            Register(typeof(FirstAid), "First Aid", "Provide a quick heal over time effect to a single target, useful for minor injuries or when in combat.", null, "Mana: 10", 21004, 9300, School.HealersLexicon);
            Register(typeof(RestorativeTouch), "Restorative Touch", "Heal an ally and temporarily increase their resistance to damage or ailments.", null, "Mana: 15", 21004, 9300, School.HealersLexicon);
            Register(typeof(HealingMeditation), "Healing Meditation", "Enter a meditative state that gradually restores health to the caster and nearby allies.", null, "Mana: 25", 21004, 9300, School.HealersLexicon);
            Register(typeof(VitalitySurge), "Vitality Surge", "Boost the maximum health of a target temporarily, increasing their survivability in battles.", null, "Mana: 20", 21004, 9300, School.HealersLexicon);
            Register(typeof(EmergencyHeal), "Emergency Heal", "A quick, but less powerful, heal that can be used in critical situations with a short cooldown.", null, "Mana: 15", 21004, 9300, School.HealersLexicon);
            Register(typeof(RevitalizingElixir), "Revitalizing Elixir", "Create a potion that, when consumed, provides a significant health boost and minor healing over time.", null, "Mana: 20", 21004, 9300, School.HealersLexicon);
            Register(typeof(HealthTransfer), "Health Transfer", "Transfer a portion of the caster's health to an injured ally, useful in emergencies.", null, "Mana: 25", 21004, 9300, School.HealersLexicon);
            Register(typeof(HealingAura), "Healing Aura", "Passively heal nearby allies over time while active, providing continuous support during extended fights.", null, "Mana: 30", 21004, 9300, School.HealersLexicon);
        }
    }
}
