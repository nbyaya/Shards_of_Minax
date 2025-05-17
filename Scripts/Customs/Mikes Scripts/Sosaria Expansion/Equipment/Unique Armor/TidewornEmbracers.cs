using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TidewornEmbracers : LeatherGloves
{
    [Constructable]
    public TidewornEmbracers()
    {
        Name = "Tideworn Embracers";
        Hue = Utility.Random(2300, 2500); // Blue-green color, reminiscent of the sea
        BaseArmorRating = Utility.RandomMinMax(20, 40); // Medium armor for gloves

        // Attributes with a focus on survivability and oceanic connection
        Attributes.DefendChance = 10;
        Attributes.BonusDex = 8;
        Attributes.BonusStam = 6;
        Attributes.RegenStam = 3;
        Attributes.LowerManaCost = 5;

        // Water and nature-based resistances
        PhysicalBonus = 15; // Enhanced physical resistance for oceanic armor
        ColdBonus = 10;     // Cold resistance from the frigid ocean environment
        PoisonBonus = 10;   // Protective against poison (sea creatures, toxins)

        // Skill Bonuses themed around nature, animals, and sea
        SkillBonuses.SetValues(0, SkillName.Veterinary, 10.0); // Helps with animal care in the wild
        SkillBonuses.SetValues(1, SkillName.Herding, 10.0);    // Suitable for guiding sea creatures or herding land animals
        SkillBonuses.SetValues(2, SkillName.Fishing, 15.0);    // Increases fishing skill, linking to the aquatic theme

        // Unique Item attachment for special properties
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TidewornEmbracers(Serial serial) : base(serial)
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
