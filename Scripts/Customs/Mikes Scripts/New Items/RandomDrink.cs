using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomDrink : Item
{
    private static string[] DrinkPart1 = new string[]
    {
        "Sparkling", "Fizzy", "Refreshing", "Cool", "Icy", "Sweet",
        "Sour", "Bitter", "Tart", "Fruity", "Exotic", "Tropical",
        "Creamy", "Frothy", "Smoky", "Peaty", "Crisp", "Dry",
        "Smooth", "Rich", "Velvety", "Robust", "Bold", "Aged",
        "Zesty", "Citrusy", "Herbal", "Spiced", "Mellow", "Effervescent",
        "Vibrant", "Luscious", "Silky", "Full-bodied", "Aromatic", "Fragrant",
        "Pungent", "Sharp", "Minty", "Vanilla", "Chocolatey", "Buttery",
        "Oaky", "Nutty", "Floral", "Honeyed", "Caramel", "Bitter-sweet",
        "Creamy", "Tangy", "Tropical", "Robust", "Mild", "Soothing",
        "Zingy", "Earthy", "Smokey", "Savory", "Tangy", "Satisfying",
        "Juicy", "Zesty", "Sizzling", "Spirited", "Bubbly", "Glowing",
        "Divine", "Radiant", "Crispy", "Succulent", "Tender", "Chilled",
        "Warm", "Steamy", "Lush", "Ripe", "Infused", "Whipped",
        "Sensational", "Mystical", "Magical", "Enchanting", "Velvet", "Gentle",
        "Crunchy", "Balanced", "Tantalizing", "Decadent", "Ethereal", "Blissful",
        "Heavenly", "Dazzling", "Glittering", "Exquisite", "Opulent", "Svelte"
    };

    private static string[] DrinkPart2 = new string[]
    {
        "Water", "Juice", "Soda", "Lemonade", "Cola", "Tea",
        "Coffee", "Ale", "Beer", "Lager", "Stout", "Cider",
        "Wine", "Champagne", "Mead", "Sake", "Cocktail", "Mocktail",
        "Smoothie", "Milkshake", "Hot Chocolate", "Eggnog", "Punch", "Grog",
        "Tonic", "Frappe", "Latte", "Espresso", "Mocha", "Americano",
        "Gin", "Vodka", "Rum", "Whiskey", "Tequila", "Brandy",
        "Liqueur", "Kombucha", "Horchata", "Chai", "Matcha", "Root Beer",
        "Ginger Beer", "Hot Toddy", "Mulled Wine", "Bellini", "Mimosa",
        "Kombucha", "Latte", "Cappuccino", "Espresso", "Malt", "Root Beer",
        "Ginger Beer", "Mojito", "Martini", "Margarita", "Daiquiri", "Piña Colada",
        "Smoothie", "Frappe", "Shake", "Refresher", "Spritzer", "Slushie",
		"Aperitif", "Aquavit", "Bourbon", "Cachaça", "Chardonnay", "Chianti",
        "Cognac", "Gewürztraminer", "Grappa", "Kirsch", "Lambrusco", "Madeira",
        "Merlot", "Moonshine", "Port", "Prosecco", "Riesling", "Rosé",
        "Sambuca", "Scotch", "Sherry", "Soju", "Syrah", "Tawny",
        "Verdejo", "Verdelho", "Verdicchio", "Vermouth", "Viognier", "Zinfandel"
    };

    private static int[] DrinkGraphics = new int[]
    {
        0x098D, 0x098E, 0x099C, 0x099D, 0x099E, 0x099F, 0x09A0, 0x09A1, 0x09A2
    };

    public static string GenerateDrinkName()
    {
        return DrinkPart1[Utility.Random(DrinkPart1.Length)] + " " + DrinkPart2[Utility.Random(DrinkPart2.Length)];
    }

    [Constructable]
    public RandomDrink() : base(DrinkGraphics[Utility.Random(DrinkGraphics.Length)])
    {
        Name = GenerateDrinkName();
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

            from.SendMessage("You enjoy the drink, feeling refreshed.");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x5A9); // Drinking sound

            Delete(); // Remove the drink after consumption
        }
        else
        {
            from.SendMessage("You are simply too full to drink any more!");
        }
    }

    public RandomDrink(Serial serial) : base(serial)
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
