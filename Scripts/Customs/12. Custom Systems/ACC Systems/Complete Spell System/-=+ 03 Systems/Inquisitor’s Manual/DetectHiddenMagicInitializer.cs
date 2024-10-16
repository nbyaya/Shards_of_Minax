using System;
using Server;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class DetectHiddenMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
			Register(typeof(CallDetectives), "Call Detectives", "Call Detectives to defend you", null, "Mana: 25", 21005, 9301, School.InquisitorsManual);
			Register(typeof(RevealingStrike), "Revealing Strike", "A powerful attack that exposes all hidden enemies within a small radius. Deals additional damage to concealed foes when revealed.", null, "Mana: 20", 21004, 9300, School.InquisitorsManual);
            Register(typeof(ShadowSever), "Shadow Sever", "A quick strike that disrupts an opponent's ability to hide or go invisible for a short duration. Causes bleeding damage over time if the target was attempting to hide.", null, "Mana: 25", 21004, 9300, School.InquisitorsManual);
            Register(typeof(IlluminatingBurst), "Illuminating Burst", "A burst of light that blinds enemies in front of the user and reveals all hidden enemies within a cone. Blinded enemies have reduced accuracy for a short time.", null, "Mana: 30", 21004, 9300, School.InquisitorsManual);
            Register(typeof(WraithMark), "Wraith Mark", "Places a magical mark on a hidden enemy, making them visible and reducing their evasion. The marked target takes extra damage from all sources for a limited time.", null, "Mana: 25", 21004, 9300, School.InquisitorsManual);
            Register(typeof(SpectralTrap), "Spectral Trap", "Sets a trap that activates upon detecting a hidden enemy. The trap immobilizes the target and reveals their position to all allies for several seconds.", null, "Mana: 30", 21004, 9300, School.InquisitorsManual);
            Register(typeof(PhantomChains), "Phantom Chains", "Conjures spectral chains that bind hidden enemies, dragging them out of their hiding places and dealing minor damage over time. Chains last for a limited duration or until broken by the enemy.", null, "Mana: 35", 21004, 9300, School.InquisitorsManual);
            Register(typeof(UnseenHunter), "Unseen Hunter", "Enhances the user's next attack to deal additional damage if it strikes a hidden or recently revealed enemy. Grants a small amount of mana back if successful.", null, "Mana: 20", 21004, 9300, School.InquisitorsManual);
            Register(typeof(EchoStrike), "Echo Strike", "A dual attack that sends out an echoing wave, dealing damage to enemies within range and causing hidden enemies to momentarily shimmer, revealing their location.", null, "Mana: 25", 21004, 9300, School.InquisitorsManual);
            Register(typeof(TrueSight), "True Sight", "Temporarily grants the user and nearby allies the ability to see through invisibility and stealth. Provides a significant advantage in PvP or PvE scenarios.", null, "Mana: 40", 21004, 9300, School.InquisitorsManual);
            Register(typeof(ArcanePerception), "Arcane Perception", "Passively increases the user's detection radius for hidden and invisible entities. Also grants a slight bonus to magic resistance while active.", null, "Mana: 15", 21004, 9300, School.InquisitorsManual);
            Register(typeof(CloakSense), "Cloak Sense", "Allows the user to sense the presence and approximate location of cloaked or invisible enemies within a larger radius, but without fully revealing them.", null, "Mana: 20", 21004, 9300, School.InquisitorsManual);
            Register(typeof(MirageWard), "Mirage Ward", "Creates a protective ward that reveals any hidden enemy entering its area and grants a temporary speed boost to allies passing through it.", null, "Mana: 30", 21004, 9300, School.InquisitorsManual);
            Register(typeof(FogOfTruth), "Fog of Truth", "Generates a mist around the user that dispels illusions and reveals hidden traps and enemies. Enemies attempting to hide within the mist will be instantly revealed.", null, "Mana: 35", 21004, 9300, School.InquisitorsManual);
            Register(typeof(ScryingEye), "Scrying Eye", "Summons a magical eye that scouts a specific area for hidden foes. Provides a visual feed to the caster, perfect for scouting.", null, "Mana: 25", 21004, 9300, School.InquisitorsManual);
            Register(typeof(VeilShatter), "Veil Shatter", "Dispels all illusions and magical effects within a radius, forcing any hidden entities to become visible and negating their stealth abilities for a short period.", null, "Mana: 40", 21004, 9300, School.InquisitorsManual);
        }
    }
}
