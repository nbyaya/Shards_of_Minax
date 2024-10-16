using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyPainting : Item
{
    private static string[] Part1 = new string[]
    {
        "Abstract", "Landscape", "Portrait", "Still Life", "Seascape", "Cityscape",
        "Surreal", "Impressionist", "Cubist", "Expressionist", "Pop Art", "Minimalist",
        "Realist", "Futurist", "Dadaist", "Neoclassical", "Romantic", "Baroque",
        "Renaissance", "Gothic", "Modern", "Contemporary", "Postmodern", "Art Nouveau",
        "Art Deco", "Fauvist", "Symbolist", "Constructivist", "Suprematist", "Bauhaus",
        "De Stijl", "Post-Impressionist", "Pre-Raphaelite", "Rococo", "Neo-Expressionist",
        "Neo-Pop", "Hyperrealist", "Photorealist", "Appropriation", "Conceptual", "Performance",
        "Installation", "Video", "Digital", "Graffiti", "Street", "Mural", "Collage", "Assemblage",
		"Abstract Expressionist", "Pointillist", "Flemish", "Byzantine", "Impressionist Landscape",
        "Cubist Portrait", "Surrealist Seascape", "Futurist Cityscape", "Dadaist Still Life",
        "Neoclassical Figure", "Romantic Landscape", "Baroque Portrait",
        "Renaissance Religious", "Gothic Architectural", "Modern Abstract", "Contemporary Figurative",
        "Postmodern Conceptual", "Art Nouveau Decorative", "Art Deco Geometric",
        "Fauvist Colorful", "Symbolist Mythological", "Constructivist Industrial",
        "Suprematist Geometric", "Bauhaus Design", "De Stijl Abstract", "Post-Impressionist Vibrant",
        "Pre-Raphaelite Detailed", "Rococo Ornate", "Neo-Expressionist Emotional",
        "Neo-Pop Cultural", "Hyperrealist Detailed", "Photorealist Precise", "Appropriation Borrowed",
        "Conceptual Idea-based", "Performance Action", "Installation Site-specific",
        "Video Moving Image", "Digital Pixelated", "Graffiti Urban", "Street Public",
        "Mural Large-scale", "Collage Mixed Media", "Assemblage Sculptural",
		"Pointillist", "Abstract Expressionist", "Neo-Plasticist", "Neo-Futurist", "Neo-Romantic",
        "Post-Minimalist", "Neo-Geo", "Op Art", "Kinetic Art", "Hard-edge Painting",
        "Lyrical Abstraction", "Color Field Painting", "Action Painting", "Tachisme",
        "Informalism", "Art Informel", "Nouveau Réalisme", "Fluxus", "Happening",
        "Arte Povera", "Neo-Dada", "Neo-Surrealism", "Meta-Expressionism",
        "Figurative Art", "Naive Art", "Primitive Art", "Outsider Art", "Folk Art",
        "Urban Art", "Visionary Art", "Intuitive Art", "Singular Art", "Art Brut"
    };

    private static string[] Part2 = new string[]
    {
        "Masterpiece", "Work of Art", "Magnum Opus", "Chef-d'œuvre", "Gem", "Jewel",
        "Treasure", "Prize", "Marvel", "Wonder", "Miracle", "Splendor",
        "Beauty", "Elegance", "Grace", "Charm", "Allure", "Fascination",
        "Captivation", "Enchantment", "Mystique", "Radiance", "Brilliance", "Luster",
		"Pièce de résistance", "Crown Jewel", "Gold Mine", "Priceless", "Invaluable",
        "Exquisite", "Sophisticated", "Rare", "Unique", "Unparalleled", "Matchless", "Peerless",
        "Superb", "Outstanding", "Remarkable", "Exceptional", "Distinguished", "Notable",
        "Famous", "Renowned", "Celebrated", "Acclaimed", "Illustrious", "Esteemed",
		"Paragon", "Pinnacle", "Crown Jewel", "Pièce de résistance", "Quintessence",
        "Exquisiteness", "Refinement", "Sophistication", "Loveliness", "Pulchritude",
        "Comeliness", "Adorableness", "Enthrallment", "Bewitchment", "Spell",
        "Magnetism", "Appeal", "Attraction", "Glow", "Shine",
        "Sparkle", "Gleam", "Sheen", "Gloss", "Polish"
    };

    private static int[] Graphics = new int[]
    {
        0x0E9F, 0x0EA0, 0x0EA1, 0x0EA2, 0x0EA5, 0x0EA6, 0x0EC8, 0x0EE7
    };

    public static string GeneratePaintingName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyPainting() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = GeneratePaintingName();
        Hue = Utility.Random(1, 3000);

        // Setting the weight to be consistent with decoration items
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
            from.SendMessage("You enjoy the painting, feeling inspired.");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x1E0); // Coin sound
        }
    }

    public RandomFancyPainting(Serial serial) : base(serial)
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
