using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFruitBasket : Item
{
    private static string[] Part1 = new string[]
    {
        "Apple", "Banana", "Cherry", "Date", "Elderberry", "Fig",
        "Grape", "Honeydew", "Kiwi", "Lemon", "Mango", "Nectarine",
        "Orange", "Peach", "Quince", "Raspberry", "Strawberry", "Tangerine",
        "Uva", "Watermelon", "Xigua", "Yellow Passion Fruit", "Zucchini",
        "Apricot", "Blueberry", "Clementine", "Dragon Fruit", "Feijoa",
        "Guava", "Huckleberry", "Jabuticaba", "Kumquat", "Lychee",
        "Mulberry", "Nashi Pear", "Olallieberry", "Pomegranate", "Quenepa",
        "Rambutan", "Salal Berry", "Tamarillo", "Ugli Fruit", "Victoria Plum",
        "Wax Apple", "Yuzu", "Zapote",
        "Ambarella", "Bilimbi", "Cupuaçu", "Durian", "Eggfruit",
        "Finger Lime", "Gac Fruit", "Hala Fruit", "Imbe", "Jujube",
        "Kiwano", "Longan", "Mangosteen", "Noni Fruit", "Olive",
        "Papaya", "Quararibea Cordata", "Roselle", "Soursop", "Tamarind",
        "Uvaria", "Vanilla Bean", "Wampee", "Xylocarp", "Yumberry",
        "Zalzalak", "Açaí", "Barbadine", "Cupuacu", "Duku", "Elaeagnus Latifolia",
        "Fennel Fruit", "Galia Melon", "Horned Melon", "Ice Cream Bean",
        "Jatoba", "Kiwiberry", "Limequat", "Muntingia Calabura", "Nance",
        "Oca", "Papino", "Quandong", "Rutabaga", "Sugar Apple",
        "Tangor", "Ulluco", "Vanilla Clamshell", "White Sapote", "Ximenia",
        "Yacón", "Zig Zag Vine Fruit"
    };

    private static string[] Part2 = new string[]
    {
        "Delight", "Surprise", "Indulgence", "Bliss", "Joy", "Pleasure",
        "Euphoria", "Ecstasy", "Harmony", "Serenity", "Luxury", "Opulence",
        "Decadence", "Rapture", "Reverie", "Extravagance", "Splendor", "Panache",
        "Elegance", "Finesse", "Glamour", "Charm", "Grandeur", "Sumptuousness",
        "Savory", "Sensation", "Satisfaction", "Exquisite", "Perfection", "Magnificence",
        "Epicurean", "Culinary", "Sumptuous", "Divine", "Aromatic", "Enchanting",
        "Gourmet", "Succulent", "Delectable", "Temptation", "Luscious", "Exotic",
        "Heavenly", "Irresistible", "Plush", "Rich", "Velvety", "Voluptuous"
    };

    public static string GenerateFruitName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFruitBasket() : base(0x0993)
    {
        Name = GenerateFruitName();
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

            from.SendMessage("You enjoy the fruit, feeling refreshed.");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x5A9); // Eating sound

            Delete(); // Remove the fruit basket after consumption
        }
        else
        {
            from.SendMessage("You are simply too full to eat any more!");
        }
    }

    public RandomFruitBasket(Serial serial) : base(serial)
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
