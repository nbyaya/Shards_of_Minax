using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyBanner : Item
{
    private static string[] Part1 = new string[]
    {
        "Lion", "Dragon", "Griffin", "Phoenix", "Unicorn", "Wolf",
        "Bear", "Eagle", "Hawk", "Falcon", "Owl", "Raven",
        "Stag", "Boar", "Horse", "Dolphin", "Shark", "Whale",
        "Skull", "Rose", "Sun", "Moon", "Star", "Tree",
        "Mountain", "Sea", "Fire", "Ice", "Storm", "Shadow",
        "Ghost", "Demon", "Angel", "Serpent", "Spider", "Scorpion",
		"Lyon", "Wyvern", "Sphinx", "Basilisk", "Pegasus", "Minotaur",
        "Panther", "Condor", "Kestrel", "Swan", "Crow", "Jackal",
        "Elk", "Buffalo", "Camel", "Octopus", "Carp", "Penguin",
        "Cross", "Anchor", "Crescent", "Flower", "Comet", "Oak",
        "Volcano", "River", "Wind", "Snow", "Lightning", "Mist",
        "Specter", "Gargoyle", "Cherub", "Hydra", "Tarantula", "Crab",
		"Dragonfly", "Tiger", "Kangaroo", "Elephant", "Turtle", "Lynx",
        "Cheetah", "Swallow", "Peacock", "Crane", "Seahorse", "Jaguar",
        "Cobra", "Lotus", "Maple", "Cedar", "Trident", "Lyre",
        "CelticKnot", "FleurDeLis", "YinYang", "Taijitu", "Inukshuk", "Totem",
        "Mandala", "Ankh", "Torii", "Dharmachakra", "Om", "Hamsa",
        "Valknut", "Mjolnir", "HelmOfAwe", "Aegishjalmur", "Vegvisir", "Triquetra",
        "RavenBanner", "DragonBanner", "LionBanner", "SunBanner", "MoonBanner", "StarBanner",
        "CrossBanner", "EagleBanner", "TigerBanner", "SerpentBanner", "SkullBanner", "RoseBanner",
		"Lynx", "Condor", "Albatross", "Swallow", "Cuckoo", "Magpie",
        "Moose", "Bison", "Dromedary", "Squid", "Salmon", "Flamingo",
        "Trident", "Harpy", "Harpyia", "Chimera", "Centaur", "HarpyEagle",
        "Cheetah", "Vulture", "Merlin", "Cygnet", "Rook", "Coyote",
        "Moose", "Bison", "Llama", "Squid", "Koi", "Albatross",
        "Totem", "CelticKnot", "FleurDeLis", "TudorRose", "Sakura", "Pine",
        "Cedar", "Lotus", "MapleLeaf", "Protea", "Fern", "Thistle",
        "Sphinx", "Gryphon", "Basilisk", "Kraken", "Manticore", "Wendigo"
    };

    private static string[] Part2 = new string[]
    {
        "Valor", "Honor", "Courage", "Glory", "Victory", "Triumph",
        "Power", "Strength", "Might", "Fury", "Wrath", "Rage",
        "Wisdom", "Knowledge", "Truth", "Justice", "Freedom", "Peace",
        "Love", "Passion", "Desire", "Dream", "Hope", "Faith",
        "Fate", "Destiny", "Chaos", "Death", "War", "Conquest",
		"Bravery", "Loyalty", "Gallantry", "Pride", "Supremacy", "Dominion",
        "Force", "Resilience", "Vigor", "Ire", "Vengeance", "Outrage",
        "Insight", "Enlightenment", "Honesty", "Equality", "Liberty", "Harmony",
        "Affection", "Ardor", "Yearning", "Vision", "Trust", "Devotion",
        "Doom", "Predestination", "Disorder", "Mortality", "Strife", "Invasion",
		"Valiant", "Noble", "Bold", "Gallant", "Heroic", "Fearless",
        "Resolute", "Steadfast", "Unyielding", "Indomitable", "Relentless", "Unstoppable",
        "Sage", "Intellect", "Prudence", "Integrity", "Fairness", "Serenity",
        "Compassion", "Fervor", "Longing", "Imagination", "Optimism", "Confidence",
        "Kismet", "Providence", "Anarchy", "Obsidian", "Warfare", "Supremacy",
        "Chivalry", "Allegiance", "Dignity", "Majesty", "Sovereignty", "Reign",
        "Potency", "Endurance", "Spirit", "Anger", "Retribution", "Indignation",
        "Discernment", "Illumination", "Sincerity", "Parity", "Autonomy", "Concord",
        "Tenderness", "Zeal", "Aspiration", "Foresight", "Reliance", "Dedication",
        "Calamity", "Inevitability", "Tumult", "Mortality", "Conflict", "Encroachment",
		"Valiance", "Gallantry", "Chivalry", "Dignity", "Mercy", "Prowess",
        "Endurance", "Fortitude", "Audacity", "Zeal", "Ardor", "Fervor",
        "Sagacity", "Intuition", "Clarity", "Fairness", "Autonomy", "Serenity",
        "Joy", "Fervor", "Longing", "Aspiration", "Belief", "Confidence",
        "Chance", "Providence", "Tumult", "Demise", "Battle", "Subjugation",
        "Fearlessness", "Allegiance", "Heroism", "Esteem", "Authority", "Sovereignty",
        "Vigilance", "Steadfastness", "Spirit", "Indignation", "Retribution", "Anger",
        "Discernment", "Illumination", "Transparency", "Parity", "Fraternity", "Unity",
        "Tenderness", "Eagerness", "Craving", "Fantasy", "Reliance", "Commitment",
        "Gloom", "Foreordination", "Anarchy", "Ephemerality", "Quarrel", "Intrusion"
    };

    private static int[] Graphics = new int[]
    {
        0x15AE, 0x15AF, 0x15B0, 0x15B1, 0x15B2, 0x15B3, 0x15B4, 0x15B5, 0x15B6, 0x15B7, 0x15B8, 0x15B9, 0x15BA, 0x15BB, 0x15BC, 0x15BD, 0x15BE, 0x15BF, 0x15C0, 0x15C1, 0x15C2, 0x15C3, 0x15C4, 0x15C5, 0x15C6, 0x15C7, 0x15C7, 0x15C9, 0x15CA, 0x15CB, 0x15CC, 0x15CD, 0x15CE, 0x15CF, 0x15D0, 0x15D1, 0x15D2, 0x15D3, 0x15D4, 0x15D5, 0x15D6, 0x15D7, 0x15D8, 0x15D9, 0x15DA, 0x15DB, 0x15DC, 0x15DD, 0x15DE, 0x15DF, 0x15E0, 0x15E1, 0x15E2, 0x15E3, 0x15E4, 0x15E5, 0x15E6, 0x15E7, 0x15E8, 0x15E9, 0x15EA, 0x15EB, 0x15EC, 0x15ED, 0x15EE, 0x15EF, 0x15F0, 0x15F1, 0x15F2, 0x15F3, 0x15F4, 0x15F5     
    };

    public static string GenerateBannerName()
    {
        return "House of " + Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyBanner() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = GenerateBannerName();
        Hue = Utility.Random(1, 3000);

        // Setting the weight to be consistent with other items
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
            from.SendMessage("You wave the banner, showing your loyalty to the lord.");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x1E0); // Coin sound
        }
    }

    public RandomFancyBanner(Serial serial) : base(serial)
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
