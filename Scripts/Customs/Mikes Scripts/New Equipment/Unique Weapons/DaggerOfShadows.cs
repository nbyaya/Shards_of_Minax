using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DaggerOfShadows : Dagger
{
    [Constructable]
    public DaggerOfShadows()
    {
        Name = "Dagger of Shadows";
        Hue = Utility.Random(850, 2000);
        MinDamage = Utility.RandomMinMax(35, 65);
        MaxDamage = Utility.RandomMinMax(75, 115);
        Attributes.BonusDex = 25;
        Attributes.WeaponSpeed = 10;
        Slayer = SlayerName.ArachnidDoom;
        WeaponAttributes.HitPoisonArea = 40;
        WeaponAttributes.HitFatigue = 40;
        SkillBonuses.SetValues(0, SkillName.Stealth, 40.0);
        SkillBonuses.SetValues(1, SkillName.Poisoning, 35.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DaggerOfShadows(Serial serial) : base(serial)
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
