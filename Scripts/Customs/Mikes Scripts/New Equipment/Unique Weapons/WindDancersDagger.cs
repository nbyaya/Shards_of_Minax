using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WindDancersDagger : Dagger
{
    [Constructable]
    public WindDancersDagger()
    {
        Name = "Wind Dancer's Dagger";
        Hue = Utility.Random(450, 2900);
        MinDamage = Utility.RandomMinMax(10, 40);
        MaxDamage = Utility.RandomMinMax(40, 70);
        Attributes.BonusDex = 10;
        Attributes.WeaponSpeed = 5;
        Slayer = SlayerName.SummerWind;
        WeaponAttributes.HitFatigue = 30;
        WeaponAttributes.BattleLust = 20;
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WindDancersDagger(Serial serial) : base(serial)
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
