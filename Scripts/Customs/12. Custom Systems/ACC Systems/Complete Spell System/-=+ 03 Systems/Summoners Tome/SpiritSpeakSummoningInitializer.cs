using System;
using Server;

namespace Server.ACC.CSS.Systems.SpiritSpeakMagic
{
    public class SpiritSpeakSummoningInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(SummonSpiritWolf), "Summon Spirit Wolf", "Summons a spectral wolf that inflicts cold damage and briefly freezes targets.", null, "Mana: 20", 21006, 9302, School.SpiritSpeak);
            Register(typeof(SummonFlameWraith), "Flame Wraith", "Summons a fiery wraith that burns enemies, causing ongoing fire damage over time.", null, "Mana: 25", 21007, 9303, School.SpiritSpeak);
            Register(typeof(SummonStoneGuardian), "Stone Guardian", "Creates a durable stone elemental guardian with high defense and a chance to stun attackers.", null, "Mana: 30", 21008, 9304, School.SpiritSpeak);
            Register(typeof(SummonStormSerpent), "Storm Serpent", "Calls forth a lightning elemental serpent whose attacks chain lightning to nearby foes.", null, "Mana: 30", 21009, 9305, School.SpiritSpeak);
            Register(typeof(SummonIceboundElemental), "Icebound Elemental", "Summons a powerful ice elemental that slows enemies and deals heavy cold damage.", null, "Mana: 30", 21010, 9306, School.SpiritSpeak);
            Register(typeof(SummonVenomSprite), "Venom Sprite", "Summons a poison elemental sprite whose attacks inflict deadly poison over time.", null, "Mana: 25", 21011, 9307, School.SpiritSpeak);
            Register(typeof(SummonAcidicElemental), "Acidic Elemental", "Creates an acid elemental that corrodes enemy armor, reducing their defense over time.", null, "Mana: 25", 21012, 9308, School.SpiritSpeak);
            Register(typeof(SummonBloodElemental), "Blood Elemental", "Summons a blood elemental that drains health from enemies and transfers it to the caster.", null, "Mana: 30", 21013, 9309, School.SpiritSpeak);
            Register(typeof(SummonVoidFiend), "Void Fiend", "Summons a shadowy creature from the void that blinds and weakens enemies with darkness.", null, "Mana: 25", 21014, 9310, School.SpiritSpeak);
            Register(typeof(SummonEtherealGuardian), "Ethereal Guardian", "Calls forth an ethereal being immune to physical attacks, dealing magical damage to foes.", null, "Mana: 30", 21015, 9311, School.SpiritSpeak);
            Register(typeof(SummonNatureDryad), "Nature Dryad", "Summons a protective dryad that heals allies and entangles enemies, immobilizing them briefly.", null, "Mana: 25", 21016, 9312, School.SpiritSpeak);
            Register(typeof(SummonMagmaColossus), "Magma Colossus", "Summons a powerful lava elemental that leaves pools of magma, damaging enemies over time.", null, "Mana: 35", 21017, 9313, School.SpiritSpeak);
            Register(typeof(SummonFrostSpecter), "Frost Specter", "Creates an icy specter that can teleport short distances, chilling enemies and reducing their speed.", null, "Mana: 25", 21018, 9314, School.SpiritSpeak);
            Register(typeof(SummonArcanePhoenix), "Arcane Phoenix", "Summons a magical phoenix whose attacks burn and dispel beneficial effects from enemies.", null, "Mana: 35", 21019, 9315, School.SpiritSpeak);
            Register(typeof(SummonQuicksilverElemental), "Quicksilver Elemental", "Summons a fluid metallic elemental that rapidly attacks multiple enemies and evades physical damage.", null, "Mana: 30", 21020, 9316, School.SpiritSpeak);
            Register(typeof(SummonSpiritBear), "Spirit Bear", "Calls forth a powerful bear spirit that deals massive damage and grants increased defense to allies.", null, "Mana: 35", 21021, 9317, School.SpiritSpeak);
        }
    }
}
