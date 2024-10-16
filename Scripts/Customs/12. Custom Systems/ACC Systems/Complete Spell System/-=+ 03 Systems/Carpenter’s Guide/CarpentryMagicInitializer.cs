using System;
using Server;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class CarpentersGuideMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(WoodenShield), "Wooden Shield", "Creates a temporary wooden shield that boosts the user's defense for a short duration.", null, "Mana: 20", 21004, 9300, School.CarpentersGuide);
            Register(typeof(SplinterShot), "Splinter Shot", "Fires a volley of sharp wooden splinters at the target.", null, "Mana: 20", 21004, 9300, School.CarpentersGuide);
            Register(typeof(Barricade), "Barricade", "Constructs a wooden barricade that blocks enemy movement in a small area.", null, "Mana: 25", 21004, 9300, School.CarpentersGuide);
            Register(typeof(ReinforcedStructure), "Reinforced Structure", "Temporarily reinforces an ally's armor with wooden planks, increasing durability.", null, "Mana: 25", 21004, 9300, School.CarpentersGuide);
            Register(typeof(SpikeTrap), "Spike Trap", "Sets a hidden wooden spike trap that injures enemies who step on it.", null, "Mana: 30", 21004, 9300, School.CarpentersGuide);
            Register(typeof(TimberBarrage), "Timber Barrage", "Calls down a hail of heavy logs from above, crushing enemies.", null, "Mana: 35", 21004, 9300, School.CarpentersGuide);
            Register(typeof(WoodlandCamouflage), "Woodland Camouflage", "Uses wooden branches and leaves to blend into the surroundings, becoming harder to detect.", null, "Mana: 20", 21004, 9300, School.CarpentersGuide);
            Register(typeof(CraftedBow), "Crafted Bow", "Quickly crafts a temporary bow from available materials for ranged attacks.", null, "Mana: 30", 21004, 9300, School.CarpentersGuide);
            Register(typeof(TimberToss), "Timber Toss", "Throws a heavy log at the target, knocking them back and stunning them briefly.", null, "Mana: 25", 21004, 9300, School.CarpentersGuide);
            Register(typeof(WoodenGolem), "Wooden Golem", "Constructs a small wooden golem to assist in combat.", null, "Mana: 40", 21004, 9300, School.CarpentersGuide);
            Register(typeof(TreeForm), "Tree Form", "The caster takes on the form of a tree, becoming immobile but gaining significant armor and health regeneration.", null, "Mana: 40", 21004, 9300, School.CarpentersGuide);
            Register(typeof(BranchBind), "Branch Bind", "Binds an enemy in place with rapidly growing roots and branches.", null, "Mana: 25", 21004, 9300, School.CarpentersGuide);
            Register(typeof(CarvedTotem), "Carved Totem", "Crafts a totem that provides a beneficial aura to allies nearby.", null, "Mana: 30", 21004, 9300, School.CarpentersGuide);
            Register(typeof(ForestsBlessing), "Forest’s Blessing", "Calls upon the spirits of the forest to heal all nearby allies.", null, "Mana: 35", 21004, 9300, School.CarpentersGuide);
            Register(typeof(SawbladeSwipe), "Sawblade Swipe", "Swings a large sawblade in a wide arc, damaging all enemies in front.", null, "Mana: 30", 21004, 9300, School.CarpentersGuide);
            Register(typeof(LumberjacksFrenzy), "Lumberjack’s Frenzy", "The caster goes into a frenzy, increasing attack speed and damage for a short duration.", null, "Mana: 25", 21004, 9300, School.CarpentersGuide);
        }
    }
}
