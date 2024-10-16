using System;
using Server;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class LumberjackingMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(LumberjackCall), "Lumberjack Call", "Calls a friendly lumberjacks to aid in battle for a short period.", null, "Duration: 1 minute\nCooldown: 15 minutes", 21005, 9301, School.WoodcuttersJournal);
			Register(typeof(TimberStrike), "Timber Strike", "Temporarily increase the speed at which trees are cut, allowing for faster resource gathering.", null, "Mana: 10", 21001, 9301, School.WoodcuttersJournal);
            Register(typeof(WoodlandCamouflage), "Woodland Camouflage", "Temporarily blend into the environment, reducing visibility and increasing stealth while in wooded areas.", null, "Mana: 12", 21002, 9302, School.WoodcuttersJournal);
            Register(typeof(ForestsBlessing), "Forest's Blessing", "Increase the yield of resources from trees, improving the quality and quantity of logs and resources gathered.", null, "Mana: 15", 21003, 9303, School.WoodcuttersJournal);
            Register(typeof(TreeRestoration), "Tree Restoration", "Heal and restore vitality to damaged or felled trees, potentially causing them to regrow or produce resources over time.", null, "Mana: 20", 21004, 9304, School.WoodcuttersJournal);
            Register(typeof(EchoOfTheForest), "Echo of the Forest", "Summon a spectral wood spirit to assist in cutting down trees, which can also provide guidance or warnings of nearby dangers.", null, "Mana: 18", 21005, 9305, School.WoodcuttersJournal);
            Register(typeof(RootedPaths), "Rooted Paths", "Temporarily cause tree roots to emerge and create barriers or obstacles for enemies in a designated area.", null, "Mana: 25", 21007, 9307, School.WoodcuttersJournal);
            Register(typeof(LumberjacksInsight), "Lumberjack’s Insight", "Reveal hidden resources and valuable materials within trees or the surrounding environment.", null, "Mana: 15", 21008, 9308, School.WoodcuttersJournal);
            Register(typeof(AxeCleave), "Axe Cleave", "Perform a powerful sweeping attack with your axe that damages all enemies in front of you.", null, "Mana: 30", 21009, 9309, School.WoodcuttersJournal);
            Register(typeof(SplinteringStrike), "Splintering Strike", "Deliver a concentrated blow that causes your enemy to bleed, dealing damage over time.", null, "Mana: 20", 21010, 9310, School.WoodcuttersJournal);
            Register(typeof(SturdyTimber), "Sturdy Timber", "Temporarily increase your own defense by channeling the strength of trees, reducing incoming damage.", null, "Mana: 18", 21011, 9311, School.WoodcuttersJournal);
            Register(typeof(TreesVengeance), "Tree’s Vengeance", "Call upon the power of nature to summon animated branches that strike and hinder enemies.", null, "Mana: 25", 21012, 9312, School.WoodcuttersJournal);
            Register(typeof(FellingFrenzy), "Felling Frenzy", "Enter a berserk state, increasing your attack speed and damage with axes for a short duration.", null, "Mana: 30", 21013, 9313, School.WoodcuttersJournal);
            Register(typeof(WoodlandAmbush), "Woodland Ambush", "Set a trap with sharpened branches that impales and slows enemies who step on it.", null, "Mana: 22", 21014, 9314, School.WoodcuttersJournal);
            Register(typeof(NaturesGrasp), "Nature’s Grasp", "Summon vines or roots from the ground to immobilize or entangle enemies within a certain radius.", null, "Mana: 25", 21015, 9315, School.WoodcuttersJournal);
            Register(typeof(AxeThrow), "Axe Throw", "Throw your axe with great force, dealing damage and possibly stunning or disorienting the target.", null, "Mana: 20", 21016, 9316, School.WoodcuttersJournal);
        }
    }
}
