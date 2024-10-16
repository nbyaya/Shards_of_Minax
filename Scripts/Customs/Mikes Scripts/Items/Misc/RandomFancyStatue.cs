using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyStatue : Item
{
    private static int[] Graphics = new int[]
    {
        0x1224, 0x1225, 0x1226, 0x1227, 0x1228, 0x12CA, 0x12CB, 0x12D8
    };

    private static string[] Part1 = new string[]
    {
        "Marble", "Granite", "Bronze", "Golden", "Silver", "Jade",
        "Obsidian", "Crystal", "Iron", "Stone", "Wooden", "Copper",
        "Brass", "Ebony", "Ivory", "Alabaster", "Onix", "Quartz",
        "Steel", "Amethyst", "Topaz", "Sapphire", "Ruby", "Emerald",
		"Porcelain", "Ceramic", "Glass", "Platinum", "Pewter", "Agate",
		"Opal", "Lapis", "Malachite", "Coral", "Amber", "Basalt",
		"Limestone", "Marble", "Teak", "Cedar", "Mahogany", "Pine",
		"Garnet", "Aquamarine", "Turquoise", "Pearl", "Jasper", "Citrine",
		"Titanium", "Bone", "Resin", "Porphyry", "Serpentine", "Gabbro",
		"Dolomite", "Quartzite", "Schist", "Gneiss", "Sandstone", "Flint",
		"Hematite", "Azurite", "Celadon", "Terracotta", "Cobalt", "Chrome",
		"Nickel", "Tin", "Zinc", "Lead", "Silicon", "Tungsten",
		"Magnesium", "Aluminum", "Bismuth", "Beryl", "Spinel", "Peridot",
		"Moonstone", "Hematite", "Tiger's Eye", "Rhodonite", "Carnelian", "Chalcedony",
		"Petalite", "Amazonite", "Kyanite", "Iolite", "Labradorite", "Sunstone",
		"Moldavite", "Tektite", "Heliotrope", "Zircon", "Sphene", "Andalusite",
		"Basalt", "Sandstone", "Travertine", "Porphyry", "Gneiss", "Schist",
		"Quartzite", "Diorite", "Gabbro", "Serpentine", "Soapstone", "Flint",
		"Chalk", "Granite", "Slate", "Shale", "Cobalt", "Nickel",
		"Tin", "Zinc", "Lead", "Bismuth", "Antimony", "Tungsten",
		"Magnesium", "Aluminum", "Titanium", "Chromium", "Manganese", "Molybdenum",
		"Tantalum", "Vanadium", "Zirconium", "Beryl", "Spinel", "Gypsum",
		"Hematite", "Azurite", "Cinnabar", "Jet", "Kyanite", "Lignite",
		"Magnetite", "Onyx", "Peridot", "Tourmaline", "Ulexite", "Vermiculite",
		"Wollastonite", "Xenolith", "Yttrium", "Zoisite"
    };

    private static string[] Part2 = new string[]
    {
        "Statue", "Sculpture", "Bust", "Figurine", "Carving", "Effigy",
        "Monument", "Relief", "Pillar", "Totem", "Idol", "Artwork",
        "Masterpiece", "Creation", "Gargoyle", "Golem", "Ornament", "Adornment",
		"Installation", "Mosaic", "Fresco", "Bas-relief", "Portrait", "Frieze",
		"Glyph", "Mural", "Intaglio", "Cameo", "Stela", "Tapestry",
		"Diorama", "Fountain", "Tableau", "Artifact", "Mannequin", "Pendant",
		"Model", "Form", "Shape", "Structure", "Composition", "Construction",
		"Assemblage", "Collage", "Montage", "Tableau", "Diorama", "Panorama",
		"Maquette", "Artefact", "Relic", "Icon", "Talisman", "Amulet",
		"Plaque", "Medallion", "Emblem", "Insignia", "Crest", "Coat of arms",
		"Monolith", "Stelae", "Cairn", "Menhir", "Dolmen", "Trilithon",
		"Petroglyph", "Pictogram", "Hieroglyph", "Ideogram", "Logogram", "Photogram",
		"Calligram", "Palimpsest", "Manuscript", "Codex", "Scroll", "Folio",
		"Assemblage", "Collage", "Construct", "Diorama", "Environment", "Happening",
		"Installation", "Intervention", "Kinetic", "Land", "Light", "Mobile",
		"Multimedia", "Performance", "Pop", "Site-specific", "Sound", "Video",
		"Abstract", "Baroque", "Cubist", "Dada", "Expressionist", "Fauvist",
		"Futurist", "Gothic", "Impressionist", "Jugendstil", "Kitsch", "Mannerist",
		"Minimalist", "Neoclassical", "Op", "Pointillist", "Quattrocento", "Rococo",
		"Romantic", "Surrealist", "Symbolist", "Ukiyo-e", "Vorticist", "Wabi-sabi",
		"Xylography", "Yaoi", "Zen"
    };

    public static string GenerateStatueName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyStatue() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = GenerateStatueName();
        Hue = Utility.Random(1, 3000);
        Weight = 5.0; // Set an appropriate weight for statues
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!from.InRange(GetWorldLocation(), 1))
        {
            from.SendLocalizedMessage(500446); // That is too far away.
        }
        else
        {
            from.SendMessage("You appreciate the art, enjoying the experience.");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
			from.PlaySound(0x1E0); // Coin sound
        }
    }

    public RandomFancyStatue(Serial serial) : base(serial)
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
