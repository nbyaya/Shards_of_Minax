using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PetalstrideSandals : Sandals
{
    [Constructable]
    public PetalstrideSandals()
    {
        Name = "Petalstride Sandals";
        Hue = 0x847; // Light natural color, earthy tone
        
        // Set attributes and bonuses
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.RegenStam = 5;
        Attributes.Luck = 50;
        Attributes.NightSight = 1;

        // Resistances (Nature-themed)
        Resistances.Physical = 5;
        Resistances.Cold = 5;
        Resistances.Poison = 15;

        // Skill Bonuses (Emphasizing nature, stealth, and agility)
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);
        SkillBonuses.SetValues(2, SkillName.Healing, 5.0); // Represents the connection to nature and healing
        SkillBonuses.SetValues(3, SkillName.Fishing, 5.0); // Ties into nature and exploration

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PetalstrideSandals(Serial serial) : base(serial)
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
