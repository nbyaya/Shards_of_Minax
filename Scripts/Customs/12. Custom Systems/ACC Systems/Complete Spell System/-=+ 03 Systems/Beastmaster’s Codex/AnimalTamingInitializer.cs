using System;
using Server;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class BeastmastersCodexInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(SummonCompanion), "Summon Companion", "Temporarily summon a powerful animal ally to aid in combat or exploration.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(AnimalEmpathy), "Animal Empathy", "Increase the loyalty and effectiveness of your tamed animals, enhancing their combat abilities and defense.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(HealingTouch), "Healing Touch", "Heal your tamed animals over time, restoring their health during and after battles.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(CommandBeast), "Command Beast", "Issue a special command that makes your tamed animal perform a unique action, such as a special attack or defensive maneuver.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(TameWithAuthority), "Tame with Authority", "Improve your chances of successfully taming higher-level or more resistant animals.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(AnimalBond), "Animal Bond", "Create a deeper bond with your tamed animals, granting them buffs and allowing them to perform special actions more frequently.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(BeastsRoar), "Beast’s Roar", "Temporarily increase the attack power and defensive capabilities of your tamed animals with a powerful roar.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(TrackPrey), "Track Prey", "Use your skill to track and find specific animals in the wild, making it easier to locate rare or valuable creatures.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(AnimalSenses), "Animal Senses", "Temporarily enhance your tamed animal’s senses, allowing it to detect hidden enemies or traps.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(BeastsAgility), "Beast’s Agility", "Increase the movement speed and evasion of your tamed animals, making them more effective in dodging attacks.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(PackLeader), "Pack Leader", "When taming multiple animals, gain the ability to command them as a pack, improving their coordination and combat efficiency.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(BeastsVigil), "Beast’s Vigil", "Grant your tamed animals a temporary aura that boosts their defense and reduces incoming damage from enemies.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(InstinctiveStrike), "Instinctive Strike", "Empower your tamed animal’s attacks with a chance to deal extra damage or apply a debuff to enemies.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(WildHarmony), "Wild Harmony", "Temporarily calm aggressive wild animals, making it easier to tame or interact with them.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(AnimalCommunication), "Animal Speaking", "Enhance your ability to communicate with your tamed animals, allowing for more complex commands and coordination.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
            Register(typeof(BeastsResilience), "Beast’s Resilience", "Increase the overall durability and health of your tamed animals, making them harder to defeat in battle.", null, "Mana: 20", 21004, 9300, School.BeastmastersCodex);
        }
    }
}
