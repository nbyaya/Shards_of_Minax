using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyIngot : Item
{
    private static string[] Metals = new string[]
    {
        "Gold", "Silver", "Iron", "Copper", "Tin", "Bronze", "Brass", "Steel",
        "Mithril", "Adamantium", "Vibranium", "Orichalcum", "Electrum", "Obsidian",
        "Titanium", "Platinum", "Palladium", "Cobalt", "Nickel", "Zinc",
        "Aluminum", "Magnesium", "Tungsten", "Lead", "Mercury", "Uranium",
        "Thorium", "Lithium", "Selenium", "Tellurium", "Iridium", "Osmium",
        "Rhodium", "Ruthenium", "Palladium", "Platinum", "Gold", "Silver",
        "Indium", "Gallium", "Germanium", "Tin", "Lead", "Bismuth", "Polonium",
        "Francium", "Radium", "Actinium", "Thorium", "Protactinium", "Uranium",
        "Neptunium", "Plutonium", "Americium", "Curium", "Berkelium", "Californium",
        "Einsteinium", "Fermium", "Mendelevium", "Nobelium", "Lawrencium", "Rutherfordium",
        "Dubnium", "Seaborgium", "Bohrium", "Hassium", "Meitnerium", "Darmstadtium",
        "Roentgenium", "Copernicium", "Nihonium", "Flerovium", "Moscovium", "Livermorium",
        "Tennessine", "Oganesson",
		"Starsteel", "Dragonite", "Shadowmetal", "Moonstone", "Skyiron", "Bloodsilver",
        "Eternium", "Aetherium", "Stormsteel", "Frostiron", "Dawnmetal", "Duskalloy",
        "Aurorium", "Abyssalite", "Celestium", "Infernium", "Galaxium", "Nebulite",
        "Quantium", "Phantomite", "Dreamsteel", "Chaosite", "Harmonium", "Lunargent",
        "Solargold", "Elysium", "Pandorium", "Prometheum", "Voidsteel", "Nexusite",
        "Radiance", "Umbra", "Crystalline", "Ebonite", "Ivorysteel", "Sylvanite",
        "Aquarion", "Pyrosteel", "Terrasteel", "Ventusite", "Aegisite", "Nimbusite",
        "Eldritchite", "Arcaneite", "Runesteel", "Mythril", "Adamantine", "Celestite",
        "Astralite", "Divinium", "Necrosteel", "Spectralite", "Etherealite", "Crimsonite",
        "Sapphiresteel", "Emeraldine", "Amethystium", "Topazite", "Rubysteel", "Diamondium",
        "Onyxite", "Jadeite", "Lapisite", "Malachite", "Quartzite", "Opalite",
        "Garnetium", "Amberite", "Turquoise", "Pearlite", "Coralite", "Agatite",
        "Jetite", "Hematite", "Tigerite", "Lazulite", "Serpentine", "Obsidianite",
        "Zirconium", "Niobium", "Tantalum", "Chromium", "Vanadium", "Molybdenum",
        "Rhenium", "Scandium", "Yttrium", "Lanthanum", "Cerium", "Praseodymium",
        "Neodymium", "Promethium", "Samarium", "Europium", "Gadolinium", "Terbium",
        "Dysprosium", "Holmium", "Erbium", "Thulium", "Ytterbium", "Lutetium",
        "Hafnium", "Tantalum", "Rhenium", "Osmium", "Iridium", "Platinum",
        "Rhodium", "Palladium", "Silver", "Gold", "Zirconium", "Niobium",
        "Molybdenum", "Technetium", "Ruthenium", "Rhodium", "Palladium", "Osmium",
        "Iridium", "Platinum", "Gold", "Mercury", "Thallium", "Lead",
        "Bismuth", "Polonium", "Astatine", "Radon", "Francium", "Radium",
        "Actinium", "Thorium", "Protactinium", "Uranium", "Neptunium", "Plutonium",
        "Americium", "Curium", "Berkelium", "Californium", "Einsteinium", "Fermium",
        "Mendelevium", "Nobelium", "Lawrencium", "Rutherfordium", "Dubnium", "Seaborgium",
        "Bohrium", "Hassium", "Meitnerium", "Darmstadtium", "Roentgenium", "Copernicium",
        "Nihonium", "Flerovium", "Moscovium", "Livermorium", "Tennessine", "Oganesson",
		"Starsteel", "Dragonite", "Shadowmetal", "Moonstone", "Skyiron", "Sungold",
        "Bloodsilver", "Frostiron", "Stormcopper", "Dawnbronze", "Duskbrass", "Aethersteel",
        "Elderium", "Celestium", "Abyssium", "Infernium", "Eternium", "Chronium",
        "Nexusite", "Voidiron", "Galaxium", "Nebulite", "Cometsteel", "Astralium",
        "Pulsarite", "Quasarite", "Novium", "Supernium", "Quantium", "Singularium",
        "Radon", "Polonium", "Astatine", "Francium", "Radium", "Actinium",
        "Protactinium", "Neptunium", "Plutonium", "Americium", "Curium", "Berkelium",
        "Californium", "Einsteinium", "Fermium", "Mendelevium", "Nobelium", "Lawrencium",
        "Rutherfordium", "Dubnium", "Seaborgium", "Bohrium", "Hassium", "Meitnerium",
        "Darmstadtium", "Roentgenium", "Copernicium", "Nihonium", "Flerovium", "Moscovium",
        "Livermorium", "Tennessine", "Oganesson", "Ununoctium", "Ununennium", "Unbinilium"
    };

    public static string GenerateIngotName()
    {
        return Metals[Utility.Random(Metals.Length)] + " Ingot";
    }

    [Constructable]
    public RandomFancyIngot() : base(0x1BF5)
    {
        Name = GenerateIngotName();
        Hue = Utility.Random(1, 3000);

        // Setting the weight to be consistent with ingots
        Weight = 5.0;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!from.InRange(GetWorldLocation(), 1))
        {
            from.SendLocalizedMessage(500446); // That is too far away.
        }
        else
        {
            from.SendMessage("You can't wait to find more of these ingots to make some armor!");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x1E0); // Coin sound
        }
    }

    public RandomFancyIngot(Serial serial) : base(serial)
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
