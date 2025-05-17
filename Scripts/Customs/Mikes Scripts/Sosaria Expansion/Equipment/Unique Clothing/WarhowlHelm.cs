using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WarhowlHelm : OrcMask
{
    [Constructable]
    public WarhowlHelm()
    {
        Name = "Warhowl Helm";
        Hue = 0x85F; // Orcish green color

        // Set attributes and bonuses
        Attributes.BonusStr = 15;
        Attributes.BonusDex = 10;
        Attributes.BonusHits = 30;
        Attributes.WeaponDamage = 15;
        Attributes.ReflectPhysical = 5;

        // Resistances
        Resistances.Physical = 15;
        Resistances.Fire = 5;
        Resistances.Cold = 5;
        Resistances.Poison = 20;
        Resistances.Energy = 10;

        // Skill Bonuses (Fitting with Orcish and combat themes)
        SkillBonuses.SetValues(0, SkillName.Swords, 10.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        SkillBonuses.SetValues(2, SkillName.Tracking, 10.0);
        SkillBonuses.SetValues(3, SkillName.Veterinary, 10.0); // Reflecting the primal connection to beasts

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WarhowlHelm(Serial serial) : base(serial)
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
