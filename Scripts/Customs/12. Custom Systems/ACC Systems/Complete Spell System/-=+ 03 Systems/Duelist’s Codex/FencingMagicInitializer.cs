using System;
using Server;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class FencingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(PiercingStrike), "Piercing Strike", "A powerful thrust that ignores a portion of the target’s armor, dealing extra damage.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(Disarm), "Disarm", "Temporarily disarms the target, causing them to drop their weapon for a short duration.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(Riposte), "Riposte", "A counter-attack that strikes back immediately after successfully parrying an attack, dealing bonus damage.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(Ensnare), "Ensnare", "A quick thrust that temporarily slows the target’s movement speed, making it harder for them to evade attacks.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(Lunge), "Lunge", "A long-reaching attack that extends the range of your weapon momentarily, allowing you to hit enemies from a greater distance.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(FencingMastery), "Fencing Mastery", "Temporarily boosts your fencing skill, increasing your chance to hit and critical strike.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(WhirlwindSlash), "Whirlwind Slash", "A spinning attack that strikes all enemies within a short radius, dealing reduced damage to each target.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(FlurryOfBlows), "Flurry of Blows", "A series of rapid strikes that increases your attack speed for a short period.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(Feint), "Feint", "A deceptive move that lowers the target’s defense and increases your chance to land a successful hit for a short time.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(QuickReflexes), "Quick Reflexes", "Temporarily enhances your agility, increasing your evasion and reducing the chance of being hit.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(PrecisionStrike), "Precision Strike", "Allows you to target specific body parts, causing additional effects such as increased bleed damage or temporary debuffs.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(Dodge), "Dodge", "Temporarily boosts your ability to avoid incoming attacks, increasing your chance to dodge.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(RallyingCry), "Rallying Cry", "A battle shout that boosts the morale of nearby allies, increasing their attack power and defense for a short period.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(TacticalRetreat), "Tactical Retreat", "Allows you to quickly disengage from combat and move to a safer location, with a temporary boost to your speed.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(Disengage), "Disengage", "A swift maneuver that lets you evade an attack and create distance between you and your opponent.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
            Register(typeof(PrecisionParry), "Precision Parry", "Enhances your ability to parry incoming attacks, with a chance to deflect and counterattack.", null, "Mana: 20", 21005, 9301, School.DuelistsCodex);
        }
    }
}
