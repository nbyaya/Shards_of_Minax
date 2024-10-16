using System;
using Server;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class ArcheryMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(PiercingShot), "Piercing Shot", "Fires an arrow with increased penetration, piercing through multiple targets in a line.", null, "Mana: 20", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(RapidFire), "Rapid Fire", "Temporarily increases the firing rate of your arrows, allowing for quick successive shots.", null, "Mana: 25", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(ExplosiveArrow), "Explosive Arrow", "An arrow imbued with explosive energy that deals area-of-effect damage upon impact.", null, "Mana: 30", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(PoisonedTip), "Poisoned Tip", "Applies a poison to the arrow that inflicts damage over time on the target.", null, "Mana: 15", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(HomingArrow), "Homing Arrow", "Fires an arrow that homes in on its target, making it harder to miss.", null, "Mana: 25", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(FrostShot), "Frost Shot", "A freezing arrow that slows down the target and deals cold damage.", null, "Mana: 20", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(FireArrow), "Fire Arrow", "A blazing arrow that sets the target on fire, causing burn damage over time.", null, "Mana: 20", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(EagleEye), "Eagle Eye", "Enhances your vision to increase the range and accuracy of your shots.", null, "Mana: 15", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(DisablingShot), "Disabling Shot", "Aimed at the legs or arms, this shot temporarily reduces the target’s movement speed or weapon effectiveness.", null, "Mana: 25", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(StealthShot), "Stealth Shot", "A shot that can be fired while hiding, dealing increased damage if the archer remains unseen.", null, "Mana: 30", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(RicochetArrow), "Ricochet Arrow", "Fires an arrow that bounces off surfaces, allowing it to hit additional targets.", null, "Mana: 25", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(PinningShot), "Pinning Shot", "An arrow that pins the target to a surface or the ground, immobilizing them temporarily.", null, "Mana: 30", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(MultiShot), "Multi-Shot", "Fires a spread of arrows that hit multiple targets in a wide arc.", null, "Mana: 35", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(Barrage), "Barrage", "A special shot that releases a volley of arrows in a concentrated area, dealing heavy damage.", null, "Mana: 40", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(TrackingArrow), "Tracking Arrow", "Marks the target with a special arrow that allows the archer to track their movements.", null, "Mana: 20", 21005, 9301, School.ArchersGrimoire);
            Register(typeof(CursedArrow), "Cursed Arrow", "An arrow infused with dark magic that weakens the target’s defenses and reduces their resistance to further attacks.", null, "Mana: 30", 21005, 9301, School.ArchersGrimoire);
        }
    }
}
