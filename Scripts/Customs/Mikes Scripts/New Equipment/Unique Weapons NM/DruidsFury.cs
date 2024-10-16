using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DruidsFury : GnarledStaff
{
    [Constructable]
    public DruidsFury()
    {
        Name = "Druid's Fury";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 20;
        Attributes.BonusDex = 15;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitFireball = 30;
        WeaponAttributes.HitPoisonArea = 40;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0);
        SkillBonuses.SetValues(1, SkillName.Veterinary, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DruidsFury(Serial serial) : base(serial)
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
