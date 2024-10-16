using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Skullcrusher : Club
{
    [Constructable]
    public Skullcrusher()
    {
        Name = "Skullcrusher";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 40;
        Attributes.WeaponDamage = 75;
        Slayer = SlayerName.SpidersDeath;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.HitFireball = 50;
        WeaponAttributes.HitPhysicalArea = 25;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Skullcrusher(Serial serial) : base(serial)
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
