using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SkyPiercer : Halberd
{
    [Constructable]
    public SkyPiercer()
    {
        Name = "Sky Piercer";
        Hue = Utility.Random(350, 2550);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(80, 130);
        Attributes.BonusDex = 10;
        Attributes.WeaponSpeed = 10;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitLightning = 35;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SkyPiercer(Serial serial) : base(serial)
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
