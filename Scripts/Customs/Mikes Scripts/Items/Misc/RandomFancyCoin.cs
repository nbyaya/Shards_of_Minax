using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyCoin : Item
{	
	private static string[] Part1 = new string[]
	{
		"Golden", "Silver", "Bronze", "Copper", "Platinum", "Mythril",
		"Mithril", "Adamant", "Runic", "Dragon", "Dwarven", "Elven",
		"Ancient", "Mystic", "Cursed", "Blessed", "Enchanted", "Radiant",
		"Shadow", "Blood", "Star", "Celestial", "Abyssal", "Infernal",
		"Frost", "Stone", "Wood", "Iron", "Steel",
		"Byzantine", "Roman", "Greek", "Egyptian", "Chinese", "Japanese",
		"Indian", "Persian", "Viking", "Celtic", "Mayan", "Aztec",
		"Mongolian", "Medieval", "Renaissance", "Victorian", "Colonial",
		"Carolingian", "Ottoman", "Mughal", "Incan", "African", "Australian",
		"Saxon", "Frankish", "Pirate", "Barbarian", "Tudor", "Imperial",
		"Nordic", "Sumerian", "Babylonian", "Assyrian", "Etruscan", "Phoenician",
		"Titanium", "Obsidian", "Ebony", "Ivory", "Jade", "Onyx",
		"Sapphire", "Ruby", "Emerald", "Diamond", "Pearl", "Amber",
		"Gothic", "Feudal", "Samurai", "Shogun", "Qing", "Ming",
		"Mauryan", "Gupta", "Spartan", "Athenian", "Inuit", "Aztec",
		"Navajo", "Zulu", "Masai", "Cherokee", "Sioux", "Apache",
		"Russian", "French", "Spanish", "Italian", "Dutch", "Swiss",
		"Polish", "Hungarian", "Czech", "Turkish", "Korean", "Thai",
		"Vietnamese", "Malaysian", "Indonesian", "Filipino", "Hawaiian", "Maori",
		"Titanium", "Nickel", "Palladium", "Tungsten", "Brass", "Electrum",
		"Obsidian", "Jade", "Pearl", "Coral", "Ivory", "Ebony",
		"Ghostly", "Spectral", "Arcane", "Divine", "Hallowed", "Sacred",
		"Demonic", "Necromantic", "Feudal", "Samurai", "Shogun", "Raj",
		"Pharaoh", "Khan", "Tsar", "Sultan", "Aztec", "Inuit",
		"Maori", "Zulu", "Cherokee", "Apache", "Inca", "Navajo",
		"Spartan", "Athenian", "Carthaginian", "Hunnic", "Gothic", "Norman",
		"Han", "Tang", "Ming", "Qing", "Safavid", "Timurid",
		"Mamluk", "Rashidun", "Umayyad", "Abbasid", "Fatimid", "Ottoman",
		"Ashanti", "Zulu", "Yoruba", "Iroquois", "Moche", "Nazca"
	};

	private static string[] Part2 = new string[]
	{
		"Piece", "Coin", "Medallion", "Token", "Talisman", "Disc",
		"Shield", "Emblem", "Badge", "Insignia", "Seal", "Mark",
		"Symbol", "Signet", "Charm", "Amulet", "Relic", "Artifact",
		"Treasure", "Fortune", "Wealth", "Riches", "Bounty", "Hoard",
		"Denarius", "Drachma", "Dinar", "Yuan", "Koban", "Rupee",
		"Rial", "Penny", "Shilling", "Krona", "Peso", "Solidus",
		"Obol", "Shekel", "Tugrik", "Ducat", "Florin", "Guinea",
		"Crown", "Thaler", "Groat", "Mite", "Zuzim", "Stater",
		"Doubloon", "Sovereign", "Bezant", "Guilder", "Talent", "Mina",
		"Stater", "Nummus", "Hryvnia", "Real", "Tari", "Pistole",
		"Dollar", "Yen", "Won", "Ruble", "Rand", "Riyal",
		"Lira", "Euro", "Franc", "Pound", "Krone", "Kuna",
		"Lev", "Zloty", "Forint", "Leu", "Dram", "Denar",
		"Kip", "Riel", "Baht", "Rupiah", "Ringgit", "Peso",
		"Bolivar", "Sol", "Quetzal", "Colon", "Cordoba", "Balboa",
		"Dong", "Kip", "Taka", "Rufiyaa", "Birr", "Cedi",
		"Dalasi", "Naira", "Shilling", "Franc", "Pula", "Loti",
		"Rand", "Metical", "Kwacha", "Pula", "Lilangeni", "Ngultrum",
		"Rupee", "Taka", "Riyal", "Dinar", "Dirham", "Rial",
		"Manat", "Tenge", "Som", "Ouguiya", "Dram", "Lari",
		"Lira", "Franc", "Mark", "Pfennig", "Ruble", "Yen",
		"Won", "Baht", "Ringgit", "Rupiah", "Peso", "Dong",
		"Riel", "Kyat", "Kip", "Taka", "Riyal", "Dirham",
		"Dinar", "Forint", "Zloty", "Koruna", "Leu", "Lev",
		"Dram", "Manat", "Tenge", "Som", "Kuna", "Lek",
		"Denar", "Lats", "Litas", "Kroon", "Tolar", "Pula",
		"Krone", "Krona", "Escudo", "Peseta", "Lire", "Drachma",
		"Fiorino", "Scudo", "Testone", "Zecchino", "Cruzado", "Pang"
	};

    public static string GenerateCoinName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyCoin() : base(Utility.RandomList(0x0EEC, 0x0EED, 0x0EEE, 0x0EEF, 0x0EF0, 0x0EF1, 0x0EF2))
    {
        Name = GenerateCoinName();
        Hue = Utility.Random(1, 3000);

        // Setting the weight to be consistent with coins
        Weight = 0.1;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!from.InRange(GetWorldLocation(), 1))
        {
            from.SendLocalizedMessage(500446); // That is too far away.
        }
        else
        {
            from.SendMessage("You feel rich!");
            from.FixedParticles(0x376A, 9, 20, 5029, EffectLayer.Head);
            from.PlaySound(0x1E0); // Coin sound
        }
    }

    public RandomFancyCoin(Serial serial) : base(serial)
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
