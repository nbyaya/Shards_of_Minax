using System;
using Server;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class BlacksmithMagicInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(CallAllies), "Call Allies", "Call Allies to defend you", null, "Mana: 25", 21005, 9301, School.SmithsLedger);
			Register(typeof(ForgefireBurst), "Forgefire Burst", "Unleashes a burst of molten metal at a target, dealing fire damage over time.", null, "Mana: 20", 21005, 9301, School.SmithsLedger);
            Register(typeof(IroncladFortification), "Ironclad Fortification", "Temporarily increases the defense of the caster or an ally, reducing incoming physical damage.", null, "Mana: 30", 21006, 9302, School.SmithsLedger);
            Register(typeof(HammerStrike), "Hammer Strike", "A powerful melee attack using a blacksmith's hammer, dealing extra blunt damage and potentially stunning the target.", null, "Mana: 25", 21007, 9303, School.SmithsLedger);
            Register(typeof(ReinforcedArmor), "Reinforced Armor", "Temporarily improves the durability and protective qualities of a piece of worn armor, reducing damage received.", null, "Mana: 20", 21008, 9304, School.SmithsLedger);
            Register(typeof(WeaponTempering), "Weapon Tempering", "Enhances a weapon for a limited time, increasing its damage output or adding elemental damage.", null, "Mana: 25", 21009, 9305, School.SmithsLedger);
            Register(typeof(MightyBlow), "Mighty Blow", "A charged attack with increased force that has a chance to knock back enemies.", null, "Mana: 30", 21010, 9306, School.SmithsLedger);
            Register(typeof(SmeltingTouch), "Smelting Touch", "Instantly repairs a damaged piece of equipment to full durability, using a small amount of resources.", null, "Mana: 15", 21011, 9307, School.SmithsLedger);
            Register(typeof(MoltenShieldSpell), "Molten Shield", "Creates a temporary shield of molten metal around the caster or an ally, absorbing a certain amount of damage before dissipating.", null, "Mana: 35", 21012, 9308, School.SmithsLedger);
            Register(typeof(AnvilOfFury), "Anvil of Fury", "Summons a spectral anvil that slams down on enemies in a small area, dealing area-of-effect (AoE) damage.", null, "Mana: 40", 21014, 9310, School.SmithsLedger);
            Register(typeof(ImbueWithStrength), "Imbue with Strength", "Increases the strength and carrying capacity of the caster or an ally for a limited time.", null, "Mana: 25", 21015, 9311, School.SmithsLedger);
            Register(typeof(ShieldBash), "Shield Bash", "A defensive maneuver using a shield or piece of armor to bash an enemy, dealing minor damage and causing a brief stun or disorientation.", null, "Mana: 20", 21016, 9312, School.SmithsLedger);
            Register(typeof(QuenchBlade), "Quench Blade", "Temporarily imbues a weapon with the chilling properties of quenching water, dealing cold damage and slowing enemies on hit.", null, "Mana: 25", 21017, 9313, School.SmithsLedger);
            Register(typeof(EnchantedSmithing), "Enchanted Smithing", "Grants a temporary boost to crafting skills, allowing the blacksmith to create higher-quality items or special enchanted gear.", null, "Mana: 30", 21018, 9314, School.SmithsLedger);
            Register(typeof(MetallicResonance), "Metallic Resonance", "Emits a resonant frequency through metal armor or weapons, causing a brief stunning effect on enemies wielding or wearing metal.", null, "Mana: 20", 21019, 9315, School.SmithsLedger);
            Register(typeof(ForgeMastersBlessing), "Forge Masters Blessing", "A powerful buff that temporarily increases the blacksmith's overall crafting efficiency, reducing resource costs and crafting times for all items.", null, "Mana: 40", 21020, 9316, School.SmithsLedger);
        }
    }
}
