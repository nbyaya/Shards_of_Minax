using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WabbajackClub : Club
{
    [Constructable]
    public WabbajackClub()
    {
        Name = "Wabbajack Club";
        Hue = Utility.Random(250, 2300);
        MinDamage = Utility.RandomMinMax(15, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.LowerManaCost = 10;
        Attributes.Luck = 100;
        WeaponAttributes.HitDispel = 50;
        WeaponAttributes.HitMagicArrow = 20;
        SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WabbajackClub(Serial serial) : base(serial)
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
