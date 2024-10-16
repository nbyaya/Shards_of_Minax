using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyMedicine : Item
{
    private static string[] Part1 = new string[]
    {
        "Ibu", "Ace", "Para", "Hydro", "Oxy", "Meth",
        "Bupre", "Morph", "Code", "Fenta", "Trama", "Dexa",
        "Keta", "Prop", "Phen", "Gaba", "Lor", "Diaz",
        "Clon", "Ondan", "Zol", "Ropi", "Alpra", "Fluni",
        "Buspi", "Amitri", "Mirtaz", "Venla", "Trazod", "Nefazod",
        "Aci", "Algi", "Brom", "Calci", "Cephal", "Chlor", "Dex", "Diazep",
        "Doxy", "Erythro", "Fluox", "Guaifen", "Hydro", "Ibupro", "Lorat",
        "Metro", "Napro", "Omepraz", "Paracet", "Penicil", "Pseudo",
        "Ranitid", "Sertral", "Tetracycl", "Venlafax", "Zolpidem",
        "Cetirizine", "Levo", "Rabeprazole", "Azithro", "Montelukast", "Pantoprazole",
        "Risperidone", "Quetiapine", "Olanzapine", "Aripiprazole", "Citalopram", "Escitalopram",
        "Duloxetine", "Metformin", "Glipizide", "Pioglitazone", "Sitagliptin", "Rosiglitazone",
        "Losartan", "Valsartan", "Telmisartan", "Amlodipine", "Hydrochlorothiazide", "Furosemide",
        "Atorvastatin", "Rosuvastatin", "Simvastatin", "Pravastatin", "Fluvastatin", "Lovastatin",
		"Cipro", "Azithro", "Amoxi", "Nitro", "Sulf", "Metform",
        "Predni", "Carbi", "Levo", "Risper", "Queti", "Olanzapine",
        "Aripiprazole", "Fluvoxamine", "Duloxetine", "Escitalopram", "Bupropion"
    };

    private static string[] Part2 = new string[]
    {
        "profen", "tamin", "codone", "nex", "cet", "dol",
        "nol", "pam", "azepam", "pin", "zolam", "zepam",
        "clone", "stan", "azodone", "zodone", "pride", "lafin",
        "prazole", "zolem", "zapine", "faxine", "zone", "xetine",
        "pirone", "pine", "zine", "zodone", "zodone", "zodone",
        "Tane", "Pam", "Mycin", "Cillin", "Fen", "Phen", "Cet", "Tussin",
        "Stat", "Tab", "Caps", "Gel", "Syrup", "Drops", "Cream", "Ointment",
        "Lotion", "Spray", "Patch", "Inhaler",
        "sartan", "mide", "statin", "glitazone", "diptin", "pril",
        "artan", "vir", "floxacin", "cyclobenzaprine", "oxetine", "irptase",
        "ate", "olol", "ine", "azole", "mab", "nib",
		"Sulfate", "Mide", "Caine", "Pen", "Quine", "Vir",
        "Glipizide", "Mab", "Nib", "Pril", "Sartan", "Tinib",
        "Citalopram", "Ramipril", "Losartan", "Metoprolol", "Hydrochlorothiazide", "Simvastatin"
    };

    private static int[] Graphics = new int[]
    {
        0x0E44, 0x0E45, 0x0E46, 0x0E47, 0x0E48, 0x0E49, 0x0E4A, 0x0E4B, 0x0E4C, 0x0E4E, 0x0E4F, 0x0E28, 0x0E26, 0x0F8F, 0x0F8C, 0x0F82, 0x182A, 0x182E, 0x1830, 0x1832, 0x1836, 0x183B, 0x1841, 0x1840
    };

    public static string GenerateMedicineName()
    {
        return Part1[Utility.Random(Part1.Length)] + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyMedicine() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = GenerateMedicineName();
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
        else
        {
            from.Hits += Utility.Random(5, 10); // Restore 5-10 hits
            from.Stam += Utility.Random(5, 10); // Restore 5-10 stamina
            from.Mana += Utility.Random(5, 10); // Restore 5-10 mana

            from.SendMessage("You enjoy the medicine, feeling refreshed.");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x5A9); // Eating sound

            Delete(); // Remove the medicine after consumption
        }
    }

    public RandomFancyMedicine(Serial serial) : base(serial)
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
