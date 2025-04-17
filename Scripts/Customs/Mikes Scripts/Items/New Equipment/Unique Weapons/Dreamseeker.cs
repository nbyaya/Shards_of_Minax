using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Dreamseeker : CampingLanturn
{
    [Constructable]
    public Dreamseeker()
    {
        Name = "Dreamseeker";
        Hue = Utility.Random(500, 2800);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.BonusMana = 15;
        Attributes.LowerManaCost = 8;
        Slayer = SlayerName.ArachnidDoom;
        WeaponAttributes.HitManaDrain = 25;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Dreamseeker(Serial serial) : base(serial)
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
