using System;
using Server;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class RemoveTrapMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(SummonAllies), "Summon Allies", "Summon Allies to defend you", null, "Mana: 25", 21005, 9301, School.TrapmastersCodex);
			Register(typeof(TrapDisarm), "Trap Disarm", "Successfully disarm a trap without triggering it, allowing you to bypass or safely remove it.", null, "Mana: 10", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(TrapDetection), "Trap Detection", "Reveal traps in a specific area or on an object, highlighting their location to avoid them.", null, "Mana: 10", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(TrapCreation), "Trap Creation", "Set a basic trap in a designated area or on an object, which can be triggered by enemies or players.", null, "Mana: 15", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(EnhancedVision), "Enhanced Vision", "Temporarily enhance your ability to spot hidden traps, increasing the radius and accuracy of your Trap Detection.", null, "Mana: 10", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(TrapAnalysis), "Trap Analysis", "Examine a trap to learn its type, effects, and how to counter or disarm it more effectively.", null, "Mana: 15", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(Camouflage), "Camouflage", "Hide a trap or yourself, making it harder for enemies to detect.", null, "Mana: 15", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(QuickRelease), "Quick Release", "Instantly disable a trap that has been triggered, reducing damage or effects.", null, "Mana: 20", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(TrapEvasion), "Trap Evasion", "Temporarily enhance your agility to avoid traps set by enemies, reducing the chance of triggering them.", null, "Mana: 20", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(TrapOffensive), "Trap Offensive", "Convert a defensive trap into a weapon, causing damage or debuffs to enemies who trigger it.", null, "Mana: 25", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(ExplosiveTrap), "Explosive Trap", "Set a powerful explosive trap that causes area-of-effect damage upon detonation.", null, "Mana: 30", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(PoisonedTrap), "Poisoned Trap", "Create a trap with a poison effect that debilitates enemies who trigger it.", null, "Mana: 25", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(FlameTrap), "Flame Trap", "Set a trap that ignites, dealing fire damage to enemies who come into contact with it.", null, "Mana: 25", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(FreezeTrap), "Freeze Trap", "Deploy a trap that freezes or slows down enemies, reducing their mobility in combat.", null, "Mana: 25", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(ShockTrap), "Shock Trap", "Create a trap that delivers an electrical shock, stunning or dealing damage to enemies who trigger it.", null, "Mana: 30", 21001, 9200, School.TrapmastersCodex);
            Register(typeof(SummoningTrap), "Summoning Trap", "Set a trap that summons a creature or ally to fight for you when triggered.", null, "Mana: 35", 21001, 9200, School.TrapmastersCodex);
        }
    }
}
