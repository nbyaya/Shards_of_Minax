using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SkyleafTreads : ElvenBoots
{
    [Constructable]
    public SkyleafTreads()
    {
        Name = "Skyleaf Treads";
        Hue = 1157;  // A natural greenish hue fitting for Elven boots.

        // Set attributes and bonuses
        Attributes.BonusDex = 10;
        Attributes.BonusStam = 10;
        Attributes.Luck = 30;


        // Resistances
        Resistances.Physical = 5;
        Resistances.Cold = 15;
        Resistances.Poison = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0);
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 5.0);

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SkyleafTreads(Serial serial) : base(serial)
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
