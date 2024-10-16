using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Thunderfury : Longsword
{
    [Constructable]
    public Thunderfury()
    {
        Name = "Thunderfury";
        Hue = Utility.Random(500, 2900);
        MinDamage = Utility.RandomMinMax(25, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.AttackChance = 10;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.SummerWind;
        WeaponAttributes.HitLightning = 30;
        WeaponAttributes.HitManaDrain = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Thunderfury(Serial serial) : base(serial)
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
