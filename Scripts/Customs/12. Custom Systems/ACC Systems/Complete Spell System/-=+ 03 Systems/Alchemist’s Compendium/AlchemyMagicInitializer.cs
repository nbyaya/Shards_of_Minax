using System;
using Server;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class AlchemyMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(SummonElementals), "Summon Elementals", "Summon Elementals to defend you", null, "Mana: 25", 21005, 9301, School.AlchemistsCompendium);
			Register(typeof(ExplosivePotion), "Explosive Potion", "Creates a potion that causes a small explosion when thrown, dealing area-of-effect damage.", null, "Mana: 20", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(TransmutationElixir), "Transmutation Elixir", "Allows the alchemist to transmute one type of material into another, such as turning iron into gold.", null, "Mana: 30", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(HealingDraught), "Healing Draught", "A powerful potion that rapidly restores health over time.", null, "Mana: 15", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(InvisibilityTonic), "Invisibility Tonic", "Grants temporary invisibility to the user or an ally, making them harder to detect.", null, "Mana: 25", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(PotionOfStrength), "Potion of Strength", "Temporarily increases the physical strength of the drinker, improving their melee attack damage.", null, "Mana: 20", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(ManaInfusion), "Mana Infusion", "Restores a portion of mana to the user or an ally, useful for spellcasters in combat.", null, "Mana: 15", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(BerserkerBrew), "Berserker Brew", "Increases the drinker's attack speed and damage but reduces their defense.", null, "Mana: 25", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(Antidote), "Antidote", "Neutralizes poisons and toxins, curing the drinker of poison effects.", null, "Mana: 20", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(NightVisionElixir), "Night Vision Elixir", "Grants the ability to see clearly in darkness for a period of time.", null, "Mana: 20", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(ElixirOfAgility), "Elixir of Agility", "Enhances the drinker's dexterity and speed, improving their dodge and attack speed.", null, "Mana: 20", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(FrostBomb), "Frost Bomb", "Creates a potion that, when thrown, creates a freezing effect in an area, slowing down and damaging enemies.", null, "Mana: 25", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(FirebreathersFlask), "Firebreathers Flask", "Allows the user to breathe fire in a cone in front of them, dealing fire damage.", null, "Mana: 30", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(GaseousForm), "Gaseous Form", "Turns the user into a gas for a short duration, allowing them to pass through small openings and avoid physical attacks.", null, "Mana: 35", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(RegenerationElixir), "Regeneration Elixir", "Gradually restores health over a period of time to the drinker or an ally.", null, "Mana: 20", 21005, 9301, School.AlchemistsCompendium);
            Register(typeof(CureAllPotion), "Cure-All Potion", "A potent potion that cures various status effects, including poison, disease, and stun.", null, "Mana: 25", 21005, 9301, School.AlchemistsCompendium);
        }
    }
}
