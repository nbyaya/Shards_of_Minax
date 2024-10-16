using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ZephyrsFury : Cutlass
{
    [Constructable]
    public ZephyrsFury()
    {
        Name = "Zephyr's Fury";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.WeaponSpeed = 30;
        Attributes.WeaponDamage = 40;
        Slayer = SlayerName.ElementalBan;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitColdArea = 50;
        WeaponAttributes.HitDispel = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ZephyrsFury(Serial serial) : base(serial)
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
