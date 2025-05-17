using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DriftcapOfRenika : Cap
{
    [Constructable]
    public DriftcapOfRenika()
    {
        Name = "Driftcap of Renika";
        Hue = Utility.Random(3000, 2300); // Ocean-like hues
        
        // Set attributes and bonuses
        Attributes.RegenStam = 3;
        Attributes.RegenMana = 3;
        Attributes.CastSpeed = 1;
        Attributes.NightSight = 1;  // Ties to the coastal regions' natural beauty and magic at night

        // Resistances
        Resistances.Physical = 5;
        Resistances.Cold = 5;
        Resistances.Energy = 5;

        // Skill Bonuses - Thematically chosen for the coastal region, natural lore, and magic
        SkillBonuses.SetValues(0, SkillName.Fishing, 10.0);  // Reflects Renika's maritime culture
        SkillBonuses.SetValues(1, SkillName.Tracking, 8.0);  // Symbolizes an explorer's need for navigation and tracking
        SkillBonuses.SetValues(2, SkillName.Meditation, 5.0);  // Encourages focus and connection with the spiritual tides

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DriftcapOfRenika(Serial serial) : base(serial)
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
