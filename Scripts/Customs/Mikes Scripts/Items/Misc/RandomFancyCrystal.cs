using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyCrystal : Item
{
    private static int[] Graphics = new int[]
    {
        0x1ECD, 0x1ED0, 0x1F19, 0x1F1C, 0x1F13, 0x02DA, 0x0E2E, 0x0F8E, 0x2FDC, 0x2FDD, 0x468A, 0x468B
    };

    private static string[] Part1 = new string[]
    {
        "Amethyst", "Emerald", "Sapphire", "Ruby", "Diamond", "Topaz",
        "Quartz", "Opal", "Jade", "Pearl", "Aquamarine", "Garnet",
        "Citrine", "Tourmaline", "Peridot", "Lapis", "Turquoise", "Onyx",
        "Jasper", "Moonstone", "Sunstone", "Bloodstone", "Tiger's Eye", "Obsidian",
        "Malachite", "Azurite", "Celestite", "Fluorite", "Rhodochrosite", "Rose Quartz",
        "Smoky Quartz", "Ametrine", "Aventurine", "Labradorite", "Carnelian", "Agate",
		"Amber", "Beryl", "Coral", "Crystal", "Flint", "Hematite",
		"Jet", "Kyanite", "Larimar", "Moldavite", "Nephrite", "Olivine",
		"Prehnite", "Serpentine", "Tanzanite", "Ulexite", "Vesuvianite", "Zircon",
		"Spinel", "Heliotrope", "Chrysoberyl", "Morganite", "Iolite", "Amazonite",
		"Charoite", "Dumortierite", "Euclase", "Feldspar", "Galena", "Hemimorphite"
    };

    private static string[] Part2 = new string[]
    {
        "Mystic", "Enchanted", "Radiant", "Luminous", "Gleaming", "Dazzling",
        "Shimmering", "Sparkling", "Glowing", "Brilliant", "Scintillating", "Resplendent",
        "Effulgent", "Lustrous", "Iridescent", "Opalescent", "Pearlescent", "Crystalline",
        "Prismatic", "Galactic", "Cosmic", "Stellar", "Nebulous", "Ethereal",
        "Astral", "Otherworldly", "Arcane", "Sorcerous", "Magical", "Mystical",
        "Elven", "Fey", "Dwarven", "Gnomish", "Halfling", "Orcish",
		"Spectral", "Phantasmal", "Elysian", "Celestial", "Siderial", "Gossamer",
		"Feywild", "Shadowfell", "Planar", "Interstellar", "Quantum", "Nexus",
		"Vortex", "Psychic", "Arcane", "Runic", "Sigil", "Talismanic",
		"Golemic", "Draconic", "Elemental", "Alchemical", "Sorcerous", "Warped",
		"Transcendent", "Singularity", "Nebula", "Pulsar", "Quasar", "Supernova"
    };

    public static string GenerateCrystalName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)] + " crystal";
    }

    [Constructable]
    public RandomFancyCrystal() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = GenerateCrystalName();
        Hue = Utility.Random(1, 3000);

        // Setting the weight to be consistent with other items
        Weight = 1.0;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!from.InRange(GetWorldLocation(), 1))
        {
            from.SendLocalizedMessage(500446); // That is too far away.
        }
        else
        {
            from.SendMessage("You find the crystal wonderful!");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x1E0); // Coin sound
        }
    }

    public RandomFancyCrystal(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
