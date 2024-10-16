using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NormanConquerorsBow : Bow
{
    [Constructable]
    public NormanConquerorsBow()
    {
        Name = "Norman Conqueror's Bow";
        Hue = Utility.Random(300, 2500);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 110);
        Attributes.BonusDex = 10;
        Attributes.WeaponSpeed = 5;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitHarm = 70;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NormanConquerorsBow(Serial serial) : base(serial)
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
