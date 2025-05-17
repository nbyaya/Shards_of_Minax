using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WolfspineMarchers : FurBoots
{
    [Constructable]
    public WolfspineMarchers()
    {
        Name = "Wolfspine Marchers";
        Hue = 0x3F4; // Dark brown, resembling wolf fur.
        
        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.RegenStam = 2;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Physical = 15;
        Resistances.Cold = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Tracking, 10.0);
        SkillBonuses.SetValues(1, SkillName.Veterinary, 5.0);
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 10.0);
        SkillBonuses.SetValues(3, SkillName.Hiding, 5.0);


        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WolfspineMarchers(Serial serial) : base(serial)
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
