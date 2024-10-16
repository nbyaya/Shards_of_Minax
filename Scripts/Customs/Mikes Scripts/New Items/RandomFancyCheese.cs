using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyCheese : Item
{
    private static string[] Part1 = new string[]
    {
        "Gouda", "Cheddar", "Brie", "Roquefort", "Parmesan", "Mozzarella",
        "Camembert", "Gorgonzola", "Havarti", "Manchego", "Fontina", "Emmental",
        "Stilton", "Provolone", "Edam", "Ricotta", "Feta", "Goat", "Burrata",
        "Asiago", "Caciocavallo", "Halloumi", "Pecorino", "Taleggio", "Wensleydale",
        "Monterey Jack", "Brick", "Blue", "Muenster", "Colby", "Pepper Jack",
        "Comté", "Raclette", "Reblochon", "Chevre", "Jarlsberg", "Limburger",
        "Oka", "Queso Fresco", "Quark", "Robiola", "Tomme", "Vacherin",
        "Zamorano",
		"Gorgonzola Dolce", "Cambozola", "Shropshire Blue", "Danish Blue",
        "Cashel Blue", "Dolcelatte", "Roquefort Société", "Fourme d'Ambert",
        "Bleu d'Auvergne", "Saint Agur", "Gavost", "Cabrales", "Valdeón",
        "Cabécou", "Crottin de Chavignol", "Pérail", "Sainte-Maure de Touraine",
        "Bûcheron", "Picodon", "Chabichou du Poitou", "Pouligny-Saint-Pierre",
        "Cœur de Neufchâtel", "Pont-l'Évêque", "Livarot", "Maroilles",
        "Langres", "Époisses de Bourgogne", "Munster", "Tête de Moine",
        "Appenzeller", "Gruyère", "Tilsit", "Esrom", "Havarti Cream", "Jarlsberg Lite",
        "Leerdammer", "Maasdammer", "Ragusano", "Caciotta", "Casciotta",
        "Formaggio di Fossa", "Majorero", "Mahón", "Manchego Añejo", "Mató",
        "Pategrás", "Queso de Bola", "Queso de Cabra al Vino", "Queso Ibérico",
        "Queso Mahón-Menorca", "Queso Manchego", "Queso de Murcia", "Queso de Murcia al Vino",
        "Queso Tetilla", "Serra da Estrela", "Torta del Casar", "Zamorano Añejo",
		"Cambozola", "Cashel Blue", "Chabichou", "Dolcelatte", "Dubliner",
        "Epoisses", "Gruyere", "Humboldt Fog", "Jindi Brie", "Kasseri",
        "Lancashire", "Mahon", "Norvegia", "Ossau-Iraty", "Pont-l'Eveque",
        "Ragusano", "Saga", "Tete de Moine", "Ubriaco", "Västerbotten",
        "Xynotyro", "Yorkshire Blue", "Ziger"
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

    public static string GenerateCheeseName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyCheese() : base(Utility.RandomBool() ? 0x097D : 0x097E)
    {
        Name = GenerateCheeseName();
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

            from.SendMessage("You enjoy the cheese, feeling refreshed.");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x5A9); // Eating sound

            Delete(); // Remove the cheese after consumption
        }
        else
        {
            from.SendMessage("You are simply too full to eat any more!");
        }
    }

    public RandomFancyCheese(Serial serial) : base(serial)
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
