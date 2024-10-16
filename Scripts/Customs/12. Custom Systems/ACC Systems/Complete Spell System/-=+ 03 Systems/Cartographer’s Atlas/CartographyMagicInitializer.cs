using System;
using Server;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class CartographyMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(FogOfWarReveal), "Fog of War Reveal", "Uncover a portion of the world map that is normally hidden, revealing terrain, creatures, and resources.", null, "Cooldown: 30 minutes", 21004, 9300, School.CartographersAtlas);
            Register(typeof(WayfindersBeacon), "Wayfinder's Beacon", "Places a magical beacon on the map visible only to the caster, providing a glowing path to a chosen location for a short duration.", null, "Duration: 10 minutes, Cooldown: 20 minutes", 21004, 9300, School.CartographersAtlas);
            Register(typeof(MaraudersMap), "Marauder’s Map", "Temporarily reveals the locations of nearby hostile creatures or players on the map.", null, "Duration: 5 minutes, Cooldown: 30 minutes", 21004, 9300, School.CartographersAtlas);
            Register(typeof(PathOfLeastResistance), "Path of Least Resistance", "Highlights the safest route on the map to a selected destination, avoiding dangerous terrain or hostile entities.", null, "Duration: 10 minutes, Cooldown: 15 minutes", 21004, 9300, School.CartographersAtlas);
            Register(typeof(CartographersCurse), "Cartographer’s Curse", "Curses a map or location, causing those who enter the area to experience confusion or disorientation.", null, "Duration: 5 minutes, Cooldown: 1 hour", 21004, 9300, School.CartographersAtlas);
            Register(typeof(ExplorersFortune), "Explorer’s Fortune", "Increases the chance of finding rare resources or treasures while exploring new areas.", null, "Duration: 30 minutes, Cooldown: 1 hour", 21004, 9300, School.CartographersAtlas);
            Register(typeof(ShroudedCartography), "Shrouded Cartography", "Conceals your location from other players or tracking spells for a period.", null, "Duration: 10 minutes, Cooldown: 45 minutes", 21004, 9300, School.CartographersAtlas);
            Register(typeof(DimensionDoor), "Dimension Door", "Creates a temporary portal to a location marked on your map.", null, "Duration: Portal remains for 2 minutes, Cooldown: 1 hour", 21004, 9300, School.CartographersAtlas);
            Register(typeof(MysticSurveyor), "Mystic Surveyor", "Instantly reveals the quality and type of resources in a nearby area.", null, "Duration: Instant, Cooldown: 10 minutes", 21004, 9300, School.CartographersAtlas);
            Register(typeof(CartographersQuill), "Cartographer’s Quill", "Allows you to instantly copy a discovered location or dungeon layout onto another player’s map.", null, "Cooldown: 15 minutes", 21004, 9300, School.CartographersAtlas);
            Register(typeof(MapOfTheLost), "Map of the Lost", "Reveals hidden or forgotten locations on the map that are usually not visible or accessible.", null, "Duration: 10 minutes, Cooldown: 1 hour", 21004, 9300, School.CartographersAtlas);
            Register(typeof(TerrainMastery), "Terrain Mastery", "Grants a speed boost when traversing specific types of terrain (e.g., forests, mountains).", null, "Duration: 15 minutes, Cooldown: 30 minutes", 21004, 9300, School.CartographersAtlas);
            Register(typeof(DeadReckoning), "Dead Reckoning", "Provides an ability to teleport to a location you have previously visited but not marked.", null, "Cooldown: 2 hours", 21004, 9300, School.CartographersAtlas);
            Register(typeof(SurveyorsInsight), "Surveyor’s Insight", "Briefly increases your vision range, allowing you to see further than usual on the map.", null, "Duration: 5 minutes, Cooldown: 20 minutes", 21004, 9300, School.CartographersAtlas);
            Register(typeof(CartographicIllusion), "Cartographic Illusion", "Creates a decoy of a map location, tricking enemies into believing there is a treasure or target there.", null, "Duration: 10 minutes, Cooldown: 1 hour", 21004, 9300, School.CartographersAtlas);
            Register(typeof(AstralNavigation), "Astral Navigation", "Allows safe passage across otherwise dangerous or blocked terrain for a short period, such as lava or water.", null, "Duration: 5 minutes, Cooldown: 1 hour", 21004, 9300, School.CartographersAtlas);
        }
    }
}
