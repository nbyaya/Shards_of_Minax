using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SeaSerpentsScarf : Bandana
{
    [Constructable]
    public SeaSerpentsScarf()
    {
        Name = "Sea Serpent’s Scarf";
        Hue = 1152; // Deep ocean blue hue for the scarf.

        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 5;
        Attributes.BonusHits = 30;
        Attributes.BonusMana = 15;

        // Resistances
        Resistances.Physical = 5;
        Resistances.Cold = 15;
        Resistances.Poison = 10;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Fishing, 10.0); // Sea-themed, enhances fishing ability.
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 5.0); // To identify ocean creatures.
        SkillBonuses.SetValues(2, SkillName.Tracking, 5.0); // Helpful in tracking ocean-based threats like sea serpents or pirates.

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SeaSerpentsScarf(Serial serial) : base(serial)
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
