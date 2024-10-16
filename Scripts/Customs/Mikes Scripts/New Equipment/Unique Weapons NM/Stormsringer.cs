using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Stormsringer : TwoHandedAxe
{
    [Constructable]
    public Stormsringer()
    {
        Name = "Stormsringer";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 10;
        Attributes.WeaponSpeed = 25;
        Slayer2 = SlayerName.DragonSlaying;
        WeaponAttributes.HitLightning = 50;
        WeaponAttributes.ResistColdBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Stormsringer(Serial serial) : base(serial)
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
