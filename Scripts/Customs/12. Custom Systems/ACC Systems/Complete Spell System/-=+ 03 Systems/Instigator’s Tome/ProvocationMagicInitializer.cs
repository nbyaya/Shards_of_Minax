using System;
using Server;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class ProvocationMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(SummonProvokers), "Summon Provokers", "Summon Provokers to defend you", null, "Mana: 25", 21005, 9301, School.InstigatorsTome);
			Register(typeof(RabbleRouser), "Rabble-Rouser", "Temporarily increase the aggression of enemies in a targeted area, causing them to attack anyone nearby, including their allies.", null, "Mana: 20", 21005, 9400, School.InstigatorsTome);
            Register(typeof(DiscordantStrike), "Discordant Strike", "A powerful attack that confuses the target, causing them to attack their own allies or become less effective in combat.", null, "Mana: 20", 21005, 9400, School.InstigatorsTome);
            Register(typeof(TauntingBlow), "Taunting Blow", "A melee or ranged attack that provokes the target, making them focus their attacks on you for a short duration.", null, "Mana: 20", 21005, 9400, School.InstigatorsTome);
            Register(typeof(BerserkShout), "Berserk Shout", "Inflict a state of frenzy on a single enemy, increasing their damage output but also making them more vulnerable to attacks.", null, "Mana: 20", 21005, 9400, School.InstigatorsTome);
            Register(typeof(ChaoticInfluence), "Chaotic Influence", "Create an aura that disrupts enemy spellcasting, reducing their effectiveness and occasionally causing spells to misfire.", null, "Mana: 20", 21005, 9400, School.InstigatorsTome);
            Register(typeof(InciteRage), "Incite Rage", "Temporarily boost the combat skills of allies in a targeted area, inspiring them to fight more fiercely.", null, "Mana: 20", 21005, 9400, School.InstigatorsTome);
            Register(typeof(ConfoundingAura), "Confounding Aura", "Generate an aura around yourself that causes confusion and disorientation in enemies, lowering their accuracy and evasive abilities.", null, "Mana: 20", 21005, 9400, School.InstigatorsTome);
            Register(typeof(RallyingCry), "Rallying Cry", "Boost the morale of your allies, restoring a small amount of health and stamina and enhancing their resistance to fear and panic effects.", null, "Mana: 20", 21005, 9400, School.InstigatorsTome);
            Register(typeof(MockingLaugh), "Mocking Laugh", "Use humor to taunt and demoralize enemies, lowering their attack and defense skills for a short period.", null, "Mana: 20", 21005, 9400, School.InstigatorsTome);
            Register(typeof(DeceptiveManeuver), "Deceptive Maneuver", "Create a temporary illusion that misleads enemies, causing them to attack phantoms or incorrect targets.", null, "Mana: 20", 21005, 9400, School.InstigatorsTome);
            Register(typeof(SleightOfHand), "Sleight of Hand", "Discreetly steal items from enemies or allies, or place traps and distractions to mislead opponents.", null, "Mana: 20", 21005, 9400, School.InstigatorsTome);
            Register(typeof(InstigateConflict), "Instigate Conflict", "Force two or more enemy groups to fight each other, causing chaos and allowing you to pick off weakened opponents.", null, "Mana: 30", 21005, 9500, School.InstigatorsTome);
            Register(typeof(EnrageAllies), "Enrage Allies", "Increase the aggression and damage output of all allies within a certain radius, but at the cost of reducing their defense.", null, "Mana: 30", 21005, 9500, School.InstigatorsTome);
            Register(typeof(MasterfulProvocation), "Masterful Provocation", "Unleash a powerful provocation that forces all enemies in a large area to focus their attacks on you, while boosting your own defense.", null, "Mana: 30", 21005, 9500, School.InstigatorsTome);
            Register(typeof(CalamitousFrenzy), "Calamitous Frenzy", "Activate a devastating effect that causes massive confusion and chaos among enemies, making them more likely to attack each other and reducing their overall effectiveness.", null, "Mana: 30", 21005, 9500, School.InstigatorsTome);
        }
    }
}
