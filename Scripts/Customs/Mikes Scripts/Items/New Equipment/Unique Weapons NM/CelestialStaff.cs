using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CelestialStaff : QuarterStaff
{
    [Constructable]
    public CelestialStaff()
    {
        Name = "Celestial Staff";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 15;
        Attributes.CastSpeed = 1;
        Slayer = SlayerName.Fey;
        WeaponAttributes.MageWeapon = -10;
        WeaponAttributes.ResistColdBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CelestialStaff(Serial serial) : base(serial)
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
