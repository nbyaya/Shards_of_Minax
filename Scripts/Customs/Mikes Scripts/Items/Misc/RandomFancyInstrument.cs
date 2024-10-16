using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyInstrument : Item
{
    private static string[] Part1 = new string[]
    {
        "Golden", "Silver", "Brass", "Wooden", "Ivory", "Ebony",
        "Rosewood", "Mahogany", "Maple", "Oak", "Cherry", "Walnut",
        "Bamboo", "Steel", "Crystal", "Jade", "Obsidian", "Pearl",
        "Sapphire", "Ruby", "Emerald", "Amethyst", "Topaz", "Onyx",
        "Platinum", "Copper", "Marble", "Granite", "Quartz", "Agate",
        "Opal", "Lapis Lazuli", "Titanium", "Jet", "Coral", "Amber",
        "Bronze", "Nickel", "Pine", "Cedar", "Aluminum", "Tungsten",
        "Silk", "Velvet", "Leather", "Canvas", "Velour", "Denim",
        "Silicone", "Polyester", "Linen", "Cashmere", "Fur", "Fleece"
    };

    private static string[] Part2 = new string[]
    {
        "Harp", "Lute", "Flute", "Drum", "Guitar", "Violin",
        "Cello", "Trumpet", "Horn", "Pipe", "Hurdy Gurdy", "Lyre",
        "PSaltery", "Dulcimer", "Tambourine", "Cithara", "Cymbal", "Bell",
        "Gong", "Marimba", "Xylophone", "Sitar", "Veena", "Koto",
        "Bassoon", "Clarinet", "Oboe", "Saxophone", "Ukulele", "Banjo",
        "Mandolin", "Accordion", "Harpsichord", "Spinet", "Timpani", "Castanets",
        "Zither", "Balalaika", "Bagpipe", "Didgeridoo", "Glockenspiel", "Theremin",
        "Sousaphone", "Mbira", "Kalimba", "Concertina", "Trombone", "Mellophone",
        "Euphonium", "Santur", "Kaval", "Bodhran", "Saz", "Clavichord",
        "Psalterium", "Duduk", "Shakuhachi", "Steelpan", "Mellophone", "Zampogna"
    };

    public static string GenerateInstrumentName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyInstrument() : base(Utility.RandomList(0x0E9C, 0x0E9D, 0x0E9E, 0x0EB1, 0x0EB2, 0x0EB3, 0x0EB4))
    {
        Name = GenerateInstrumentName();
        Hue = Utility.Random(1, 3000);

        // Setting the weight to be consistent with musical instruments
        Weight = 2.0;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!from.InRange(GetWorldLocation(), 1))
        {
            from.SendLocalizedMessage(500446); // That is too far away.
        }
        else
        {
            from.SendMessage("You play a wonderful tune on the " + Name + ".");
            from.FixedParticles(0x376A, 10, 15, 5002, EffectLayer.Waist);
            from.PlaySound(0x1F0); // Music box sound
        }
    }

    public RandomFancyInstrument(Serial serial) : base(serial)
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
