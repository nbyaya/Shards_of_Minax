using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChillrendLongsword : Longsword
{
    [Constructable]
    public ChillrendLongsword()
    {
        Name = "Chillrend Longsword";
        Hue = Utility.Random(450, 2490);
        MinDamage = Utility.RandomMinMax(20, 65);
        MaxDamage = Utility.RandomMinMax(65, 95);
        Attributes.BonusDex = 10;
        Attributes.WeaponSpeed = 5;
        Slayer = SlayerName.WaterDissipation;
        WeaponAttributes.HitColdArea = 50;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChillrendLongsword(Serial serial) : base(serial)
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
