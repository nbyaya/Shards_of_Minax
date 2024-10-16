using System;
using Server;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class TrackingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(SummonRangers), "Summon Rangers", "Summon Rangers to defend you", null, "Mana: 25", 21005, 9301, School.TrackersGuide);
			Register(typeof(EagleEye), "Eagle Eye", "Enhances the player's ability to spot hidden or distant targets. Increases the range at which they can detect creatures or players by a certain amount.", null, "Mana: 20", 21004, 9300, School.TrackersGuide);
            Register(typeof(TrailOfShadows), "Trail of Shadows", "Allows the player to leave a false trail, misleading enemies and making it difficult for them to track the players movements.", null, "Mana: 30", 21004, 9300, School.TrackersGuide);
            Register(typeof(SilentFootsteps), "Silent Footsteps", "Reduces the noise made while moving, making it harder for enemies to detect the player.", null, "Mana: 20", 21004, 9300, School.TrackersGuide);
            Register(typeof(Camouflage), "Camouflage", "Temporarily blend into the surroundings, making the player harder to spot by enemies while stationary.", null, "Mana: 25", 21004, 9300, School.TrackersGuide);
            Register(typeof(NaturesVeil), "Natures Veil", "Summons a protective aura that hides the player from enemy tracking abilities for a short period.", null, "Mana: 35", 21004, 9300, School.TrackersGuide);
            Register(typeof(AnimalBond), "Animal Bond", "Communicate with animals to gather information about the presence of nearby creatures or players.", null, "Mana: 20", 21004, 9300, School.TrackersGuide);
            Register(typeof(QuickReflexes), "Quick Reflexes", "Increases movement speed and agility for a short duration, allowing the player to evade enemies more effectively.", null, "Mana: 25", 21004, 9300, School.TrackersGuide);
            Register(typeof(AmbushStrike), "Ambush Strike", "Launch a surprise attack from stealth, dealing increased damage to the target if the player is hidden or undetected.", null, "Mana: 30", 21004, 9300, School.TrackersGuide);
            Register(typeof(HuntersMark), "Hunters Mark", "Marks a target, making them visible and easier to track. Increases damage dealt to the marked target and reveals their location on the player's map.", null, "Mana: 20", 21004, 9300, School.TrackersGuide);
            Register(typeof(PredatorsPounce), "Predators Pounce", "Enables a sudden leap towards a target, dealing damage and stunning them briefly.", null, "Mana: 30", 21004, 9300, School.TrackersGuide);
            Register(typeof(TrappingSnare), "Trapping Snare", "Deploys a snare trap on the ground that slows and damages enemies who walk over it.", null, "Mana: 25", 21004, 9300, School.TrackersGuide);
            Register(typeof(TrackingShot), "Tracking Shot", "Fires a projectile that follows the target for a short duration, making it easier to hit them and revealing their location.", null, "Mana: 30", 21004, 9300, School.TrackersGuide);
            Register(typeof(StalkersStrike), "Stalkers Strike", "A melee attack that deals extra damage if the player has been tracking the target for a while.", null, "Mana: 25", 21004, 9300, School.TrackersGuide);
            Register(typeof(SurveillanceSphere), "Surveillance Sphere", "Deploys a magical sphere that reveals the presence of nearby enemies and their movements within a radius.", null, "Mana: 40", 21004, 9300, School.TrackersGuide);
            Register(typeof(DisorientingShout), "Disorienting Shout", "A powerful shout that disorients enemies within a certain range, reducing their ability to track or attack the player effectively.", null, "Mana: 35", 21004, 9300, School.TrackersGuide);
        }
    }
}
