using System;
using Server;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class AnimalLoreMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(BeastsInsight), "Beast’s Insight", "Gain detailed information about an animal's strengths, weaknesses, and behavior.", null, "Mana: 10", 21005, 9400, School.BestiaryTome);
            Register(typeof(FamiliarBond), "Familiar Bond", "Temporarily forge a stronger bond with a familiar animal, increasing its loyalty and effectiveness in combat.", null, "Mana: 15", 21005, 9400, School.BestiaryTome);
            Register(typeof(AnimalEmpathy), "Animal Empathy", "Calm or enrage an animal, influencing its behavior or reaction toward the player.", null, "Mana: 12", 21005, 9400, School.BestiaryTome);
            Register(typeof(NaturesAlly), "Nature’s Ally", "Summon a temporary ally from the animal kingdom to assist in combat or exploration.", null, "Mana: 20", 21005, 9400, School.BestiaryTome);
            Register(typeof(PredatorsFocus), "Predator’s Focus", "Enhance your ability to track and locate wild animals in the vicinity.", null, "Mana: 10", 21005, 9400, School.BestiaryTome);
            Register(typeof(BeastWhisperer), "Beast Whisperer", "Communicate with animals to gather information or receive guidance on nearby dangers.", null, "Mana: 15", 21005, 9400, School.BestiaryTome);
            Register(typeof(CompanionsRoar), "Companion’s Roar", "Boost the morale and combat effectiveness of your animal companions.", null, "Mana: 18", 21005, 9400, School.BestiaryTome);
            Register(typeof(Wildcraft), "Wildcraft", "Utilize parts from slain animals to craft special items or enhancements.", null, "Mana: 25", 21005, 9400, School.BestiaryTome);
            Register(typeof(InstinctiveReflexes), "Instinctive Reflexes", "Improve your evasion and reaction speed when dealing with wild animals.", null, "Mana: 14", 21005, 9400, School.BestiaryTome);
            Register(typeof(TamingMastery), "Taming Mastery", "Increase your success rate and reduce the time required to tame new animals.", null, "Mana: 20", 21005, 9400, School.BestiaryTome);
            Register(typeof(BeastlyResilience), "Beastly Resilience", "Temporarily grant yourself or your companions enhanced resistance to damage from animal attacks.", null, "Mana: 16", 21005, 9400, School.BestiaryTome);
            Register(typeof(PredatoryStrike), "Predatory Strike", "Channel the ferocity of wild animals into a powerful, nature-themed attack.", null, "Mana: 22", 21005, 9400, School.BestiaryTome);
            Register(typeof(AnimalsGrace), "Animal's Grace", "Enhance your movement and agility, mimicking the grace of certain animals.", null, "Mana: 12", 21005, 9400, School.BestiaryTome);
            Register(typeof(HealingTouch), "Healing Touch", "Heal or cure ailments of your animal companions or yourself using natural remedies.", null, "Mana: 18", 21005, 9400, School.BestiaryTome);
            Register(typeof(NaturesCall), "Nature’s Call", "Summon a group of wild animals to assist you in battle or to create a temporary diversion.", null, "Mana: 25", 21005, 9400, School.BestiaryTome);
            Register(typeof(BeastlyLore), "Beastly Lore", "Gain a temporary buff or bonus based on the type of animal currently aiding or accompanying you.", null, "Mana: 15", 21005, 9400, School.BestiaryTome);
        }
    }
}
