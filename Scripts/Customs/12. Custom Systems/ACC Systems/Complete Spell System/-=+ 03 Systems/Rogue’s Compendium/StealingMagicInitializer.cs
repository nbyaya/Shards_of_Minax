using System;
using Server;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class StealingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallRogues), "Call Rogues", "Call Rogues to defend you", null, "Mana: 25", 21005, 9301, School.RoguesCompendium);
			Register(typeof(ShadowStrike), "Shadow Strike", "Instantly attack a target from the shadows, dealing bonus damage and briefly stunning them.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(DisorientingThrust), "Disorienting Thrust", "A quick, disorienting strike that reduces the target's accuracy and evasion for a short time.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(SmokeBomb), "Smoke Bomb", "Throw a smoke bomb to obscure vision, reducing the accuracy of all enemies in the area and allowing for a quick escape.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(ThievingSwipe), "Thieving Swipe", "A fast attack that steals a small amount of gold from the target while dealing damage.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(Backstab), "Backstab", "A powerful attack performed from behind that deals increased damage and applies a bleed effect.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(DaggerDance), "Dagger Dance", "Perform a rapid series of attacks that damage all nearby enemies, with a chance to disarm them.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(PanicTrap), "Panic Trap", "Place a trap on the ground that, when triggered, causes the target to flee in fear for a short duration.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(InvisibilityCloak), "Invisibility Cloak", "Activate an invisibility cloak that makes you invisible for a short time or until you attack.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(Pickpocket), "Pickpocket", "Attempt to steal an item or gold from a target without them noticing. Success chance increases with skill level.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(ShadowMeld), "Shadow Meld", "Blend into the shadows to become harder to detect, increasing your chance to avoid detection and ambush enemies.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(DisguiseSelf), "Disguise Self", "Change your appearance to mimic another player or creature, reducing the chances of being recognized.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(EscapeArtist), "Escape Artist", "Quickly free yourself from binds, traps, or grapples and gain a short burst of speed.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(TrapMastery), "Trap Mastery", "Set traps with increased effectiveness. Traps have a higher chance to succeed and deal more damage.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(EvasionBoost), "Evasion Boost", "Temporarily increase your dodge and evasion skills, making you harder to hit for a short duration.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
            Register(typeof(InformationGatherer), "Information Gatherer", "Gain insights about a target's weaknesses and resistances by observing them, improving your chances to exploit them in combat.", null, "Mana: 10", 21004, 9300, School.RoguesCompendium);
        }
    }
}
