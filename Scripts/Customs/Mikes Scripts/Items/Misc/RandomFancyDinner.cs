using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyDinner : Item
{
    private static int[] Graphics = new int[]
    {
        0x09BB, 0x09BC, 0x09C9, 0x09D8, 0x09DC, 0x09DE, 0x09E1, 0x09ED
    };
	
	private static string[] Part1 = new string[]
	{
		"Spicy", "Sweet", "Sour", "Bitter", "Umami", "Salty",
		"Smoky", "Tangy", "Zesty", "Aromatic", "Piquant", "Savory",
		"Hearty", "Rich", "Delicate", "Exotic", "Succulent", "Tender",
		"Juicy", "Crispy", "Crunchy", "Flavorful", "Delicious", "Scrumptious",
		"Tart", "Pungent", "Fragrant", "Robust", "Mellow", "Creamy",
		"Sharp", "Fiery", "Velvety", "Rustic", "Nutty", "Buttery",
		"Herbaceous", "Piquant", "Earthy", "Sweet-and-sour", "Spicy-hot", "Tangy-sweet",
		"Savory-umami", "Rich-and-hearty", "Light-and-delicate", "Exotic-and-aromatic", "Succulent-and-tender", "Juicy-and-crispy",
		"Fresh", "Sizzling", "Citrusy", "Refreshing", "Smokey", "Peppery",
		"Savory-sweet", "Savory-spicy", "Tangy-spicy", "Fruity", "Sour-sweet", "Balsamic",
		"Creamy-spicy", "Smoked", "Robust-flavored", "Savory-rich", "Savory-sweet-and-spicy", "Sweet-savory",
		"Zesty-citrus", "Fragrant-spiced", "Buttery-smooth", "Tangy-sweet-and-sour", "Tender-crispy", "Caramelized",
		"Garlicky", "Lemony", "Garlicky-spicy", "Sweet-and-spicy", "Fiery-sweet", "Garlicky-buttery",
		"Honey-glazed", "Soy-glazed", "Teriyaki-glazed", "Ginger-infused", "Citrus-marinated", "Pepper-crusted",
		"Herb-infused", "Sesame-crusted", "Lime-infused", "Lemon-peppered", "Spicy-peanut", "Savory-sesame",
		"Barbecue-seasoned", "Smoky-barbecue", "Barbecue-glazed", "Chipotle-seasoned", "Smoky-chipotle", "Honey-chipotle",
		"Lemon-herb", "Garlic-herb", "Herb-crusted", "Herb-roasted", "Herb-marinated", "Roasted-garlic",
		"Citrusy-herb", "Peppercorn-crusted", "Peppered", "Pepper-spiced", "Savory-peppery", "Spicy-pepper",
		"Pepper-seasoned", "Pepper-infused", "Pepper-glazed", "Pepper-crusted", "Pepper-marinated", "Cajun-spiced"
	};

	private static string[] Part2 = new string[]
	{
		"Feast", "Banquet", "Dish", "Cuisine", "Delight", "Treat",
		"Meal", "Repast", "Platter", "Spread", "Gourmet", "Culinary",
		"Masterpiece", "Creation", "Specialty", "Extravaganza", "Indulgence", "Sensation",
		"Fiesta", "Gala", "Celebration", "Festivity", "Exhibition", "Presentation",
		"Assortment", "Array", "Collection", "Assembly", "Ensemble", "Composition",
		"Work of art", "Magnum opus", "Pièce de résistance", "Gastronomic delight", "Epicurean pleasure", "Palatable sensation",
		"Menu", "Course", "Dining experience", "Gourmet meal", "Elegant dish", "Fine dining",
		"Banquet spread", "Feast of flavors", "Exquisite meal", "Grand celebration", "Gastronomic adventure", "Taste sensation",
		"Gourmet delight", "Culinary masterpiece", "Delicious ensemble", "Luxurious treat", "Sumptuous meal", "Epicurean delight",
		"Exquisite cuisine", "Gastronomic feast", "Savoring delight", "Opulent meal", "Decadent delight", "Delectable banquet",
		"Fine repast", "Elegant dining", "Grand banquet", "Decadent feast", "Sensory indulgence", "Savory spread",
		"Tantalizing experience", "Gourmet sensation", "Gastronomic experience", "Epicurean adventure", "Fine dining experience", "Luxurious dining",
		"Delightful banquet", "Exquisite repast", "Gourmet extravaganza", "Palate pleaser", "Gourmet creation", "Culinary delight",
		"Sensational meal", "Gourmet masterpiece", "Culinary delight", "Epicurean sensation", "Gastronomic pleasure", "Taste extravaganza",
		"Indulgent dining", "Elegant repast", "Gourmet experience", "Culinary adventure", "Exquisite dining", "Fine cuisine",
		"Gourmet treat", "Savoring experience", "Luxurious banquet", "Delicious feast", "Sumptuous spread", "Opulent banquet"
	};

    public static string GenerateDinnerName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyDinner() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = GenerateDinnerName();
        Hue = Utility.Random(1, 3000);
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

            from.SendMessage("You enjoy the dinner, feeling refreshed.");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x5A9); // Eating sound

            Delete(); // Remove the dinner after consumption
        }
        else
        {
            from.SendMessage("You are simply too full to eat any more!");
        }
    }

    public RandomFancyDinner(Serial serial) : base(serial)
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
