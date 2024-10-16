using System;
using Server;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class WrestlingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(PowerStrike), "Power Strike", "Delivers a devastating blow with enhanced damage, capable of stunning the opponent.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(Grapple), "Grapple", "Engages the opponent in a grapple, reducing their ability to move and attack effectively.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(BreakFree), "Break Free", "Allows the user to escape from grapples or holds, improving mobility and positioning.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(Counterattack), "Counterattack", "Executes a quick counter-attack when successfully defending against an opponent's strike, dealing damage back.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(ArmLock), "Arm Lock", "Applies a debilitating arm lock, temporarily reducing the opponent's weapon skill or ability to use certain items.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(Throw), "Throw", "Throws the opponent, potentially causing damage and disorienting them, reducing their accuracy.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(SweepingKick), "Sweeping Kick", "A low kick that knocks down enemies in front of you, causing them to lose their balance and leaving them vulnerable.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(AdrenalineRush), "Adrenaline Rush", "Increases the user's speed and strength for a short duration, enhancing both offensive and defensive capabilities.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(DefensiveStance), "Defensive Stance", "Increases the user's defense, reducing incoming damage and improving the chance to evade attacks.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(EnhancedRecovery), "Enhanced Recovery", "Speeds up healing or recovery from injuries, reducing downtime between fights.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(IntimidatingPresence), "Intimidating Presence", "Creates a fearsome aura that lowers the attack power or morale of nearby enemies.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(QuickReflexes), "Quick Reflexes", "Enhances reaction time, making it easier to dodge incoming attacks and react quickly to threats.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(ShieldBash), "Shield Bash", "Uses the user's shield or arms to bash opponents, causing temporary disorientation or a chance to break their defense.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(ResilientArmor), "Resilient Armor", "Temporarily hardens the user’s skin or armor, providing extra protection against damage for a short period.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(EmpathicTouch), "Empathic Touch", "Allows the user to heal minor injuries of allies or themselves through physical contact, providing a small amount of health restoration.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
            Register(typeof(TrainingRegimen), "Training Regimen", "Provides a temporary boost to the user’s wrestling skill, improving accuracy and effectiveness in combat for a limited time.", null, "Mana: 15", 21004, 9300, School.GrapplersManual);
        }
    }
}
