using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThistlelineGreaves : StuddedHaidate
{
    [Constructable]
    public ThistlelineGreaves()
    {
        Name = "Thistleline Greaves";
        Hue = Utility.Random(1, 1000); // Randomly assigning color for uniqueness
        BaseArmorRating = Utility.RandomMinMax(15, 50); // Adjust base armor rating for balanced rarity

        // Adding attributes
        Attributes.BonusDex = 15; // Boosts Dexterity, fitting for agility-based characters
        Attributes.BonusStam = 10; // Increases stamina, aiding with sprinting and evading
        Attributes.DefendChance = 10; // Increases defense chance, making the wearer harder to hit

        // Adding skill bonuses related to agility, stealth, and outdoor survival
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0); // Enhances Stealth, useful for sneaking through dangerous areas
        SkillBonuses.SetValues(1, SkillName.Anatomy, 10.0); // Useful for assessing enemy weaknesses, fitting for a scout role
        SkillBonuses.SetValues(2, SkillName.Tracking, 15.0); // Ties to the nature theme, helping to track enemies or prey

        // Elemental resistances
        ColdBonus = 5;  // Minor cold resistance, perhaps from the thistle-like armor texture
        PhysicalBonus = 10; // Enhances physical defense, given the studded nature

        // Attach the XML level item for potential dynamic effects
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThistlelineGreaves(Serial serial) : base(serial)
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
