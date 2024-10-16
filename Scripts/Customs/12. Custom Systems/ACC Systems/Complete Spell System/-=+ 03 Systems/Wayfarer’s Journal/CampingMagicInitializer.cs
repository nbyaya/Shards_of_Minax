using System;
using Server;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class CampingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CampfireCraft), "Campfire Craft", "Instantly create a campfire at your location that restores a small amount of health and stamina over time to nearby allies.", null, "Cooldown: 5 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(WildernessStealth), "Wilderness Stealth", "Grants temporary invisibility to the caster when in a forest or wilderness area, enhancing stealth and avoiding enemy detection.", null, "Duration: 30 seconds\nCooldown: 3 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(NaturesRefuge), "Natures Refuge", "Creates a protective circle around the camp that reduces incoming damage for all allies within the area.", null, "Duration: 20 seconds\nCooldown: 6 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(Trailblazer), "Trailblazer", "Increases the movement speed of all party members when traveling through rough terrain or forests.", null, "Duration: 2 minutes\nCooldown: 10 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(HerbalRemedy), "Herbal Remedy", "Allows the player to gather herbs from the surrounding area to create a potion that cures poison or diseases.", null, "Cooldown: 2 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(ForagersBounty), "Foragers Bounty", "Summon a bounty of food and drink that provides a temporary buff to stamina regeneration and hunger satisfaction.", null, "Duration: 10 minutes\nCooldown: 15 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(BonfireAura), "Bonfire Aura", "Enhances a campfire to emit an aura that increases mana regeneration for nearby spellcasters.", null, "Duration: 1 minute\nCooldown: 5 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(CamouflageShelter), "Camouflage Shelter", "Constructs a temporary shelter that grants a bonus to armor and reduces ranged damage from enemies.", null, "Duration: 3 minutes\nCooldown: 8 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(NightVision), "Night Vision", "Temporarily enhances the vision of the caster and allies, allowing them to see in darkness as if it were daylight.", null, "Duration: 1 minute\nCooldown: 3 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(FirestartersEmber), "Firestarters Ember", "Ignites a small area, causing a fire that damages enemies who walk through it.", null, "Duration: 10 seconds\nCooldown: 7 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(WayfindersGuidance), "Wayfinder's Guidance", "Reveals the path ahead, marking a safe route and revealing hidden traps or enemies.", null, "Duration: 30 seconds\nCooldown: 10 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(AnimalCall), "Animal Call", "Calls a friendly animal companion to aid in battle for a short period.", null, "Duration: 1 minute\nCooldown: 15 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(TinderboxIgnite), "Tinderbox Ignite", "Instants light any torch or campfire without needing kindling or a fire source, useful in dark areas.", null, "Cooldown: 2 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(WoodlandShield), "Woodland Shield", "Temporarily grants a bark skin effect to the caster, increasing defense against physical attacks.", null, "Duration: 30 seconds\nCooldown: 5 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(HearthguardWarding), "Hearthguard Warding", "Places a ward around the campfire that deters hostile creatures from approaching.", null, "Duration: 2 minutes\nCooldown: 10 minutes", 21005, 9301, School.WayfarersJournal);
            Register(typeof(LoneWanderersWisdom), "Lone Wanderer's Wisdom", "Grants a temporary boost to all skills related to survival, such as Archery, Forensic Evaluation, and Tracking.", null, "Duration: 5 minutes\nCooldown: 20 minutes", 21005, 9301, School.WayfarersJournal);
        }
    }
}
