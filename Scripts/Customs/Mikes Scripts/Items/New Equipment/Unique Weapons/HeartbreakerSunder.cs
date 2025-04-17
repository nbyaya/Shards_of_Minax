using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HeartbreakerSunder : NecromancersStaff
{
    [Constructable]
    public HeartbreakerSunder()
    {
        Name = "Heartbreaker Sunder";
        Hue = Utility.Random(300, 2500);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.BonusStr = 20;
        Attributes.WeaponSpeed = 5;
        Slayer = SlayerName.EarthShatter;
        WeaponAttributes.HitHarm = 25;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HeartbreakerSunder(Serial serial) : base(serial)
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
