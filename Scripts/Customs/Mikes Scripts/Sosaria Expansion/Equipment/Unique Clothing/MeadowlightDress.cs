using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MeadowlightDress : PlainDress
{
    [Constructable]
    public MeadowlightDress()
    {
        Name = "Meadowlight Dress";
        Hue = 0x66A; // Light, flowery hue reminiscent of a meadow
        Weight = 2.0;

        // Set attributes and bonuses
        Attributes.BonusHits = 10;
        Attributes.BonusStam = 5;
        Attributes.BonusMana = 10;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 5;
        Resistances.Cold = 15;
        Resistances.Poison = 10;
        Resistances.Energy = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 10.0);   // Fits the natural, meadow theme
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);       // Healing powers in tune with nature
        SkillBonuses.SetValues(2, SkillName.Cooking, 10.0);       // Artisanal, nature-based cooking
        SkillBonuses.SetValues(3, SkillName.Tailoring, 10.0);     // The dress is crafted with a tailored, floral touch
        SkillBonuses.SetValues(4, SkillName.Veterinary, 5.0);     // Ties to caring for animals

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MeadowlightDress(Serial serial) : base(serial)
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
