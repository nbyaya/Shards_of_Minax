using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CelestialDivide : Pitchfork
{
    [Constructable]
    public CelestialDivide()
    {
        Name = "Celestial Divide";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(55, 95);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.Luck = 100;
        Attributes.ReflectPhysical = 15;
        Slayer = SlayerName.SummerWind;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitMagicArrow = 55;
        WeaponAttributes.HitLeechStam = 20;
        SkillBonuses.SetValues(0, SkillName.Archery, 15.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CelestialDivide(Serial serial) : base(serial)
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
