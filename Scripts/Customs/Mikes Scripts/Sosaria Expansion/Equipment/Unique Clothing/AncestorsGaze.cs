using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AncestorsGaze : TribalMask
{
    [Constructable]
    public AncestorsGaze()
    {
        Name = "Ancestor's Gaze";
        Hue = 1152;  // A darker, earthy tone

        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 5;
        Attributes.BonusInt = 5;
        Attributes.BonusHits = 10;
        Attributes.BonusMana = 15;

        // Resistances
        Resistances.Physical = 10;
        Resistances.Fire = 5;
        Resistances.Cold = 5;
        Resistances.Poison = 10;
        Resistances.Energy = 8;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 20.0);    // Enhanced communication with ancestral spirits
        SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);        // Better ability to track and connect with nature
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 15.0);      // Improved understanding of animals and beasts of the land

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AncestorsGaze(Serial serial) : base(serial)
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
