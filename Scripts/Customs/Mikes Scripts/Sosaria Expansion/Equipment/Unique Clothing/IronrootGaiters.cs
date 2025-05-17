using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class IronrootGaiters : ThighBoots
{
    [Constructable]
    public IronrootGaiters()
    {
        Name = "Ironroot Gaiters";
        Hue = 1157;  // A natural, earthy color (dark green/brown)

        // Set attributes and bonuses
        Attributes.BonusStr = 10;
        Attributes.BonusDex = 5;
        Attributes.BonusHits = 15;
        Attributes.RegenHits = 2;
        Attributes.RegenStam = 2;

        // Resistances
        Resistances.Physical = 15;  // These are durable boots made for tough terrain
        Resistances.Fire = 5;       // A slight resistance to heat from the land
        Resistances.Cold = 10;      // Keeping the wearer's feet warm in cold climates
        Resistances.Poison = 10;    // Earth-based boots offer resistance to poison

        // Skill Bonuses (Thematically fitting with toughness, nature, and survival)
        SkillBonuses.SetValues(0, SkillName.Mining, 10.0);      // Reflects the toughness and connection to the earth
        SkillBonuses.SetValues(1, SkillName.Lumberjacking, 15.0); // The boots are perfect for navigating wooded terrains
        SkillBonuses.SetValues(2, SkillName.Anatomy, 5.0);       // Enhanced knowledge of body and survival
        SkillBonuses.SetValues(3, SkillName.Tracking, 10.0);      // Helping with navigation through rough areas

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public IronrootGaiters(Serial serial) : base(serial)
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
