using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomTrophy : Item
{
    private static int[] TrophyGraphics = new int[]
    {
        0x1E60, 0x1E61, 0x1E62, 0x1E63, 0x1E64, 0x1E65, 0x1E66, 0x1E68, 0x1E69, 0x1E6A, 0x1E6B, 0x1E6C, 0x1E6D
    };

    private static string[] TrophyWords1 = new string[]
    {
        "Valor", "Honor", "Courage", "Bravery", "Gallantry", "Heroism",
        "Fearlessness", "Daring", "Chivalry", "Prowess", "Skill", "Mastery",
        "Triumph", "Victory", "Conquest", "Achievement", "Accomplishment", "Success",
		"Valiance", "Boldness", "Fortitude", "Intrepidity", "Mettle", "Grit",
		"Resilience", "Tenacity", "Excellence", "Superiority", "Distinction", "Eminence",
		"Glory", "Prestige", "Esteem", "Acclaim", "Merit", "Virtue",
		"Perseverance", "Determination", "Endurance", "Strength", "Might", "Power",
		"Brilliance", "Genius", "Wisdom", "Knowledge", "Insight", "Intelligence",
		"Innovation", "Creativity", "Ingenuity", "Originality", "Imagination", "Vision",
		"Leadership", "Influence", "Authority", "Command", "Guidance", "Direction",
		"Compassion", "Kindness", "Empathy", "Sympathy", "Benevolence", "Charity",
		"Integrity", "Honesty", "Loyalty", "Trust", "Reliability", "Dependability"
    };

    private static string[] TrophyWords2 = new string[]
    {
        "Award", "Trophy", "Prize", "Medal", "Honor", "Accolade",
        "Laurel", "Crown", "Coronet", "Diadem", "Tiara", "Wreath",
        "Badge", "Emblem", "Insignia", "Token", "Memento", "Souvenir",
		"Reward", "Plaque", "Cup", "Shield", "Scepter", "Rosette",
		"Garland", "Circlet", "Crest", "Seal", "Symbol", "Sign",
		"Memorial", "Keepsake", "Remembrance", "Relic", "Artifact", "Talisman",
		"Commendation", "Citation", "Decoration", "Distinction", "Honorarium", "Tribute",
		"Testimonial", "Certificate", "Diploma", "Degree", "Title", "Designation",
		"Encomium", "Eulogy", "Panegyric", "Paean", "Ode", "Hymn",
		"Monument", "Statue", "Bust", "Plinth", "Pedestal", "Column",
		"Memento", "Token", "Reminder", "Souvenir", "Trinket", "Knickknack",
		"Legacy", "Heritage", "Bequest", "Heirloom", "Relic", "Treasure"
    };

    public static string GenerateTrophyName()
    {
        return TrophyWords1[Utility.Random(TrophyWords1.Length)] + " " + TrophyWords2[Utility.Random(TrophyWords2.Length)];
    }

    [Constructable]
    public RandomTrophy() : base(TrophyGraphics[Utility.Random(TrophyGraphics.Length)])
    {
        Name = GenerateTrophyName();
        Hue = Utility.Random(1, 3000);
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
            from.SendMessage("You feel proud of your accomplishment.");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x1E0); // Coin sound
        }
    }

    public RandomTrophy(Serial serial) : base(serial)
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
