using System;
using Server;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class LockpickingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallLocksmiths), "Call Locksmiths", "Call Locksmiths to defend you", null, "Mana: 25", 21005, 9301, School.LocksmithsCodex);
			Register(typeof(SwiftUnlock), "Swift Unlock", "Instantly unlocks a target chest or door with a high success rate.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(TrapDisarm), "Trap Disarm", "Disarms traps on chests or doors, preventing damage when the lock is picked.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(SilentPick), "Silent Pick", "Allows the player to pick locks without making any noise, avoiding detection.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(LockInsight), "Lock Insight", "Reveals the contents of a locked chest or container before attempting to unlock it.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(LockFreeze), "Lock Freeze", "Temporarily freezes a lock, making it impossible for other players to tamper with it for a short duration.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(TrapDetection), "Trap Detection", "Highlights nearby traps and hazards, making them visible even if they are normally hidden.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(QuickHands), "Quick Hands", "Reduces the time required to pick a lock, increasing speed and efficiency.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(LockingStrike), "Locking Strike", "Temporarily locks an opponent's ability to use certain abilities or items by using a special attack with a lockpicking tool.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(LockpickingThrow), "Lockpicking Throw", "Throws a specially prepared lockpicking tool to immobilize or disorient an opponent temporarily.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(Breach), "Breach", "Uses advanced techniques to forcefully open a locked container or door, causing a small explosion that can damage nearby enemies.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(KeybladeAttack), "Keyblade Attack", "Wields a magically enhanced lockpicking tool as a weapon, dealing additional damage to enemies.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(SecurityVeil), "Security Veil", "Creates a temporary barrier that protects the player from damage or debuffs while picking locks or disarming traps.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(Sabotage), "Sabotage", "Inflicts a debuff on an enemy that makes their locks or traps easier to pick, reducing their effectiveness.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(StealthAssailant), "Stealth Assailant", "Allows the player to use lockpicking tools to silently strike an enemy from a hidden position, causing surprise damage.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
            Register(typeof(LockingCounter), "Locking Counter", "Automatically counters an enemy's attempts to lock the player in place or trap them, allowing for a quick escape or retaliation.", null, "Mana: 10", 21001, 9301, School.LocksmithsCodex);
        }
    }
}
