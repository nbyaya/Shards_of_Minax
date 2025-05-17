using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DuskfeastBlouse : FormalShirt
{
    [Constructable]
    public DuskfeastBlouse()
    {
        Name = "Duskfeast Blouse";
        Hue = 1102; // Dark, elegant hue with subtle light reflections, fitting for a formal evening.

        // Set attributes and bonuses
        Attributes.BonusMana = 5;
        Attributes.BonusStam = 10;
        Attributes.RegenHits = 3;
        Attributes.RegenStam = 3;

        // Skill Bonuses (thematically related to charm, diplomacy, and social gatherings)
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 10.0);
        SkillBonuses.SetValues(1, SkillName.Begging, 5.0);
        SkillBonuses.SetValues(2, SkillName.Snooping, 5.0); // Fits with social interaction themes
        SkillBonuses.SetValues(3, SkillName.Discordance, 5.0); // Custom skill, if applicable, focusing on negotiation

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DuskfeastBlouse(Serial serial) : base(serial)
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
