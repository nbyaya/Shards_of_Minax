using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyPottery : Item
{
    private static string[] Part1 = new string[]
    {
        "Glazed", "Rustic", "Elegant", "Ancient", "Intricate", "Delicate",
        "Robust", "Vibrant", "Colorful", "Handcrafted", "Exotic", "Antique",
        "Polished", "Decorative", "Unique", "Artistic", "Traditional", "Contemporary",
        "Artisanal", "Ceramic", "Porcelain", "Stoneware", "Earthenware", "Terracotta",
        "Glossy", "Matte", "Patterned", "Textured", "Hand-painted", "Ornate",
        "Fragile", "Durable", "Timeless", "Classic", "Modern", "Minimalist",
        "Sculpted", "Engraved", "Chiseled", "Adorned", "Embellished", "Opulent",
        "Whimsical", "Regal", "Majestic", "Serene", "Mystical", "Enchanting",
        "Ethereal", "Enigmatic", "Aged", "Vintage", "Fancy", "Quirky",
        "Eclectic", "Wholesome", "Charming", "Radiant", "Sparkling", "Majestic",
        // Add more pottery adjectives here
    };

    private static string[] Part2 = new string[]
    {
        "Vase", "Pot", "Jug", "Bowl", "Plate", "Dish", "Cup", "Mug", "Jar", "Urn",
        "Statue", "Figurine", "Tile", "Ornament", "Pitcher", "Teapot", "Candle Holder",
        "Sculpture", "Bust", "Planter", "Ashtray", "Coaster", "Creamer", "Sugar Bowl",
        "Cereal Bowl", "Salad Bowl", "Fruit Bowl", "Serving Platter", "Gravy Boat",
        "Soup Tureen", "Casserole Dish", "Pie Dish", "Cookie Jar", "Tea Cup", "Coffee Mug",
        "Decanter", "Chalice", "Sake Set", "Incense Burner", "Oil Lamp", "Flower Pot",
        "Amphora", "Goblet", "Condiment Set", "Trivet", "Tureen", "Carafe",
        "Tobacco Jar", "Sake Cup", "Salt Cellar", "Pepper Shaker", "Butter Dish", "Fondue Pot",
        // Add more pottery nouns here
    };

    public static string GeneratePotteryName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyPottery() : base(Utility.RandomList(0x0B45, 0x0B46, 0x0B47, 0x0B48))
    {
        Name = GeneratePotteryName();
        Hue = Utility.Random(1, 3000);

        // Setting the weight to be consistent with pottery items
        Weight = 2.0;
    }

    public RandomFancyPottery(Serial serial) : base(serial)
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
