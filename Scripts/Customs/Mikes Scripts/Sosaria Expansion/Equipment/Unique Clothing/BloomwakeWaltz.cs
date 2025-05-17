using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BloomwakeWaltz : FloweredDress
{
    [Constructable]
    public BloomwakeWaltz()
    {
        Name = "Bloomwake Waltz";
        Hue = 1157;  // A gentle pastel hue that complements floral themes

        // Set attributes and bonuses
        Attributes.BonusHits = 10;
        Attributes.BonusStam = 10;
        Attributes.BonusMana = 30;
        Attributes.Luck = 50;
        Attributes.EnhancePotions = 10;

        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 5;
        Resistances.Cold = 10;
        Resistances.Poison = 15;
        Resistances.Energy = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0);  // Deep connection with nature
        SkillBonuses.SetValues(1, SkillName.Cooking, 10.0);  // The art of creating nourishing beauty
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);  // A tranquil energy that reflects grace
        SkillBonuses.SetValues(3, SkillName.Tailoring, 20.0);  // Expertise in working with fabrics and floral designs
        SkillBonuses.SetValues(4, SkillName.Musicianship, 10.0);  // A sense of rhythm and harmony that embodies the dance of flowers

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BloomwakeWaltz(Serial serial) : base(serial)
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
