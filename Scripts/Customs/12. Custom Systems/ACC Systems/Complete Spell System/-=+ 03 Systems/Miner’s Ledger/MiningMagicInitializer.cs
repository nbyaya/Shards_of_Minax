using System;
using Server;

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class MiningMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            // Utility Abilities
            Register(typeof(StoneGolemSpell), "Stone Golem", "Summon a stone golem to assist you in combat or to act as a temporary tank.", null, "Mana: 25", 21010, 9300, School.MinersLedger);
			Register(typeof(RockformShield), "Rockform Shield", "Temporarily encase yourself in rock, increasing defense and reducing damage taken.", null, "Mana: 20", 21001, 9300, School.MinersLedger);
            Register(typeof(EnergizedPickaxe), "Energized Pickaxe", "Temporarily boosts the mining speed and yields by using magical energy.", null, "Mana: 15", 21002, 9300, School.MinersLedger);
            Register(typeof(OreSense), "Ore Sense", "Detect hidden ores and valuable minerals within a certain radius.", null, "Mana: 10", 21003, 9300, School.MinersLedger);
            Register(typeof(Prospect), "Prospect", "Identify the quality and quantity of minerals in a specific area before mining.", null, "Mana: 25", 21004, 9300, School.MinersLedger);
            Register(typeof(GemCraft), "Gem Craft", "Transform raw gems into useful items or jewelry with enhanced properties.", null, "Mana: 30", 21005, 9300, School.MinersLedger);
            Register(typeof(Tunnel), "Tunnel", "Dig a short tunnel through rock, creating a passageway or escaping a confined space.", null, "Mana: 35", 21006, 9300, School.MinersLedger);
            Register(typeof(QuarryCreation), "Quarry Creation", "Create a temporary quarry for efficient mining in an area, increasing mineral yield.", null, "Mana: 40", 21007, 9300, School.MinersLedger);
            Register(typeof(GeologistsInsight), "Geologists Insight", "Provides information on the geological stability of the area to avoid collapses.", null, "Mana: 15", 21008, 9300, School.MinersLedger);

            // Combat Abilities
            Register(typeof(RockSmash), "Rock Smash", "Use your pickaxe to smash rocks in the vicinity, causing damage to enemies and possibly stunning them.", null, "Mana: 20", 21009, 9300, School.MinersLedger);
            Register(typeof(OreBomb), "Ore Bomb", "Throw a volatile ore fragment that explodes on impact, dealing area-of-effect damage to enemies.", null, "Mana: 30", 21011, 9300, School.MinersLedger);
            Register(typeof(Earthquake), "Earthquake", "Create a localized earthquake, causing tremors that damage and potentially knock down enemies.", null, "Mana: 40", 21012, 9300, School.MinersLedger);
            Register(typeof(ShatteringStrike), "Shattering Strike", "Deliver a powerful strike with your pickaxe that causes a rock to shatter, dealing heavy damage to a single target.", null, "Mana: 25", 21013, 9300, School.MinersLedger);
            Register(typeof(GraniteBarrier), "Granite Barrier", "Erect a barrier of granite that provides temporary cover or blocks enemy movement.", null, "Mana: 35", 21014, 9300, School.MinersLedger);

            // Hybrid Abilities
            Register(typeof(CaveCollapse), "Cave Collapse", "Trigger a minor cave-in to block off enemy paths or trap enemies in an area.", null, "Mana: 30", 21015, 9300, School.MinersLedger);
            Register(typeof(FissureTrap), "Fissure Trap", "Create a fissure in the ground that slows and damages enemies who walk over it.", null, "Mana: 25", 21016, 9300, School.MinersLedger);
        }
    }
}
