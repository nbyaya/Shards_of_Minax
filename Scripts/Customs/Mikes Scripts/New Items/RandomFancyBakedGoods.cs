using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyBakedGoods : Item
{
    private static string[] Part1 = new string[]
    {
        "Apple", "Blueberry", "Cherry", "Chocolate", "Coconut", "Coffee",
        "Custard", "Lemon", "Pecan", "Pumpkin", "Raspberry", "Strawberry",
        "Vanilla", "Walnut", "Banana", "Carrot", "Cheese", "Cinnamon",
        "Ginger", "Honey", "Orange", "Pineapple", "Poppy", "Raisin",
        "Rum", "Almond", "Apricot", "Blackberry", "Caramel", "Fig",
        "Grapefruit", "Hazelnut", "Kiwi", "Lime", "Mango", "Maple",
        "Nutmeg", "Oatmeal", "Peanut", "Quince", "Rose", "Sesame",
        "Tangerine", "Ube", "Violet", "Watermelon", "Xmas", "Zucchini",
        "Cinnamon", "Coconut", "Cream", "Date", "Ginger", "Honey",
        "Maple", "Mincemeat", "Pineapple", "Poppy", "Raisin", "Red Velvet",
        "Rhubarb", "Rum", "Sour Cream", "Spice", "Yellow",
        "Avocado", "Baileys", "Basil", "Blackcurrant", "Butterscotch", "Cherry",
        "Chili", "Citrus", "Cointreau", "Crème Brûlée", "Dulce de Leche", "Earl Grey",
        "Espresso", "Fudge", "Galangal", "Green Tea", "Hibiscus", "Icing",
        "Jam", "Kahlúa", "Lavender", "Marzipan", "Nougat", "Oreo",
        "Pistachio", "Quark", "Ricotta", "Saffron", "Tiramisu", "Umami",
        "Vanilla Bean", "Whiskey", "Yogurt", "Zest", "Pumpkin Spice", "Matcha",
        "Cardamom", "Buttermilk", "Cranberry", "Hazelnut Praline", "Ganache",
        "Black Forest", "Passion Fruit", "Baklava", "Peach", "Mint", "Coconut Cream",
        "Lemon Zest", "Cherry Blossom", "Earl Grey Tea", "Blueberry Lemon", "Raspberry Rose",
        "Mocha", "Salted Caramel", "Pineapple Coconut", "Lavender Honey", "Chai Spice",
        "Toffee", "Earl Grey Lavender", "Strawberry Basil", "Chocolate Orange", "Almond Joy",
        "Lemon Poppy Seed", "Gingerbread", "Peach Cobbler", "Raspberry White Chocolate",
        "Mango Tango", "Coconut Lime", "Lemon Blueberry", "Orange Cranberry", "Pistachio Rosewater"
    };

    private static string[] Part2 = new string[]
    {
        "Pie", "Cake", "Muffin", "Tart", "Cupcake", "Biscuit",
        "Bun", "Croissant", "Danish", "Donut", "Eclair", "Fritter",
        "Galette", "Horn", "Jelly Roll", "Kuchen", "Loaf", "Macaron",
        "Napoleon", "Oblea", "Pancake", "Quiche", "Roulade", "Scone",
        "Torte", "Upside-down Cake", "Vacherin", "Waffle", "Xmas Pudding", "Yule Log",
        "Babka", "Cannoli", "Flan", "Gâteau", "Hamentashen", "Kouign-amann",
        "Linzer", "Madeleine", "Panna Cotta", "Quesillo", "Rugelach", "Soufflé",
        "Tres Leches", "Um Ali", "Vla", "Woopie Pie", "Xuxu", "Zeppole",
        "Brownie", "Shortcake", "Eclairs", "Strudel", "Focaccia", "Brioche",
        "Crêpe", "Éclair", "Strudel", "Mille-feuille", "Gingerbread Cookies", "Savory Pie",
        "Lemon Bars", "Caramel Apple Pie", "Cinnamon Roll", "Strawberry Shortcake", "Cheesecake",
        "Chocolate Chip Cookies", "Macarons", "Pumpkin Roll", "Cannoli Cream Puffs", "Chocolate Soufflé",
        "Peach Cobbler", "Black Forest Cake", "Apple Crisp", "Lemon Meringue Pie", "Fruit Tart",
        "Chocolate Truffles", "Blueberry Crumble Bars", "Raspberry Cheesecake", "Pecan Pie Bars",
        "Chocolate Covered Strawberries", "Banana Bread", "Key Lime Pie", "Peach Pie", "Chocolate Lava Cake"
    };


    private static int[] Graphics = new int[]
    {
        0x1040, 0x1041, 0x1042, 0x1044, 0x103C, 0x09E9, 0x09EA, 0x09EB, 0x160B
    };

    public static string GenerateBakedGoodsName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyBakedGoods() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = GenerateBakedGoodsName();
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

            from.SendMessage("You enjoy the treat, feeling refreshed.");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x5A9); // Eating sound

            Delete(); // Remove the baked goods after consumption
        }
        else
        {
            from.SendMessage("You are simply too full to eat any more!");
        }
    }

    public RandomFancyBakedGoods(Serial serial) : base(serial)
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
