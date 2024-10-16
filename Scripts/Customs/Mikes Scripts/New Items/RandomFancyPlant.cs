using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyPlant : Item
{
    private static string[] Part1 = new string[]
    {
        "Blossoming", "Flowering", "Budding", "Sprouting", "Growing", "Thriving",
        "Prospering", "Flourishing", "Verduous", "Lush", "Fertile", "Vibrant",
        "Radiant", "Colorful", "Exotic", "Rare", "Unusual", "Magical", "Enchanted",
        "Mystical", "Mythical", "Legendary", "Ancient", "Sacred", "Golden",
        "Silver", "Bronze", "Emerald", "Ruby", "Sapphire", "Amethyst", "Topaz",
        "Onyx", "Jade", "Pearl", "Diamond", "Crystal", "Mystic", "Enchanted",
        "Ancient", "Twilight", "Dawn", "Sun", "Moon", "Star", "Shadow", "Frost",
        "Fire", "Thunder", "Nature", "Spirit", "Ghost", "Dragon", "Elven", "Dwarven",
        "Gnomish", "Orcish", "Trollish", "Goblin", "Fey", "Celestial", "Infernal",
        "Abyssal", "Ethereal", "Celestial", "Astral", "Autumn", "Spring", "Winter",
        "Summer", "Dusk", "Morning", "Nocturnal", "Eternal", "Divine", "Secret",
        "Hidden", "Lost", "Forbidden", "Wild", "Pure", "Haunted", "Enigma", "Chaos",
        "Dream", "Nightmare", "Hallowed", "Cursed", "Blessed", "Starlit", "Moonlit",
        "Sunlit", "Shadowy", "Frosty", "Fiery", "Thundering", "Whispering", "Singing",
        "Dancing", "Ancestral", "Primordial", "Cosmic", "Galactic", "Stellar", "Nebula",
        "Comet", "Planetary", "Lunar", "Solar", "Eclipse", "Galactic", "Quantum",
        "Nano", "Micro", "Macro", "Multi", "Omni", "Ultra", "Mega", "Giga", "Tera",
		"Crimson", "Azure", "Indigo", "Violet", "Jadeite", "Obsidian", "Opal",
		"Eternal", "Divine", "Arcane", "Cosmic", "Stellar", "Galactic", "Nebula",
		"Dusk", "Aurora", "Eclipse", "Misty", "Foggy", "Dewy", "Glowing", "Shimmering",
		"Glistening", "Sparkling", "Whispering", "Singing", "Dancing", "Laughing",
		"Weeping", "Haunted", "Cursed", "Blessed", "Hallowed", "Forbidden", "Lost",
		"Forgotten", "Wild", "Untamed", "Primal", "Elemental", "Sylvan", "Feathered",
		"Scaled", "Feytouched", "Shadowy", "Snowy", "Icy", "Fiery", "Stormy", "Marine",
		"Desert", "Jungle", "Mountain", "Valley", "River", "Ocean", "Meadow", "Forest"
    };

    private static string[] Part2 = new string[]
    {
        "Herb", "Flower", "Blossom", "Bloom", "Bud", "Sprout", "Shrub", "Bush",
        "Tree", "Vine", "Fern", "Moss", "Grass", "Cactus", "Succulent", "Bamboo",
        "Reed", "Rush", "Sedge", "Lily", "Rose", "Orchid", "Daisy", "Tulip", "Ivy",
        "Petunia", "Pansy", "Marigold", "Sunflower", "Lavender", "Basil", "Mint",
        "Thyme", "Rosemary", "Sage", "Clover", "Honeysuckle", "Jasmine", "Magnolia",
        "Pine", "Oak", "Maple", "Birch", "Cedar", "Palm", "Willow", "Cherry", "Plum",
        "Peach", "Apricot", "Apple", "Pear", "Lemon", "Lime", "Orange", "Grape",
        "Berry", "Strawberry", "Blueberry", "Raspberry", "Blackberry", "Cranberry",
        "Melon", "Watermelon", "Cantaloupe", "Honeydew", "Pumpkin", "Gourd", "Squash",
        "Cucumber", "Carrot", "Potato", "Tomato", "Pepper", "Eggplant", "Corn", "Rice",
        "Wheat", "Barley", "Oats", "Rye", "Quinoa", "Chia", "Flax", "Hemp", "Tobacco",
        "Cotton", "Algae", "Kelp", "Mushroom", "Fungus", "Yew", "Ash", "Elm", "Hickory",
        "Hazel", "Walnut", "Chestnut", "Beech", "Sycamore", "Magnolia", "Cypress",
        "Sequoia", "Spruce", "Fir", "Hemlock", "Yucca", "Agave", "Aloe", "Aspen",
        "Bamboo", "Banana", "Cactus", "Coconut", "Fig", "Ginkgo", "Grape", "Kiwi",
        "Mango", "Olive", "Papaya", "Pecan", "Persimmon", "Pineapple", "Pomegranate",
        "Quince", "Tamarind", "Teak", "Yew",
		"Petal", "Leaf", "Stem", "Root", "Seed", "Cone", "Nut", "Fruit", "Berry",
		"Branch", "Twig", "Trunk", "Bark", "Thorn", "Vein", "Frond", "Bulb", "Tuber",
		"Crop", "Grain", "Hay", "Straw", "Lawn", "Turf", "Sod", "Meadow", "Grove",
		"Orchard", "Field", "Prairie", "Moors", "Thicket", "Glade", "Hollow", "Dell",
		"Wood", "Copse", "Spinney", "Brake", "Swamp", "Marsh", "Bog", "Fen", "Reef",
		"Shore", "Beach", "Dune", "Cliff", "Ridge", "Peak", "Range", "Island", "Oasis"
    };

    private static int[] Graphics = new int[]
    {
        0x0C4F, 0x0C52, 0x0C57, 0x0C5C, 0x0C63, 0x0C6A, 0x0C83, 0x0C86, 0x0C6D, 0x0C70, 0x0C72, 0x0C77, 0x0C8B, 0x0C8D, 0x0C8F, 0x0C93, 0x0C94, 0x0C97, 0x0C9F, 0x0CA5, 0x0CA7, 0x0CC9, 0x0D0A, 0x0D0B, 0x0D13, 0x0D16, 0x0D25, 0x0D26, 0x0D27, 0x0D2A, 0x0D2E, 0x0D30
    };

    public static string GeneratePlantName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyPlant() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = GeneratePlantName();
        Hue = Utility.Random(1, 3000);

        // Setting the weight to be consistent with food items
        Weight = 1.0;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!from.InRange(GetWorldLocation(), 1))
        {
            from.SendLocalizedMessage(500446); // That is too far away.
        }
        else if (from.Hunger < 20)
        {
            from.Hunger += 1;
            from.Hits += Utility.Random(5, 10); // Restore 5-10 hits
            from.Stam += Utility.Random(5, 10); // Restore 5-10 stamina
            from.Mana += Utility.Random(5, 10); // Restore 5-10 mana

            from.SendMessage("You enjoy the plant, feeling refreshed.");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x5A9); // Eating sound

            Delete(); // Remove the plant after consumption
        }
        else
        {
            from.SendMessage("You are simply too full to eat any more!");
        }
    }

    public RandomFancyPlant(Serial serial) : base(serial)
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
