using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SamsonsJawbone : Club
{
    [Constructable]
    public SamsonsJawbone()
    {
        Name = "Samson's Jawbone";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.BonusStr = 15;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitPhysicalArea = 30;
        WeaponAttributes.BloodDrinker = 10;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SamsonsJawbone(Serial serial) : base(serial)
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
