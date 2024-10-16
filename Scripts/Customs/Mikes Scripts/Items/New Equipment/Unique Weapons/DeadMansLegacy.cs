using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DeadMansLegacy : Bow
{
    [Constructable]
    public DeadMansLegacy()
    {
        Name = "Dead Man's Legacy";
        Hue = Utility.Random(400, 2900);
        MinDamage = Utility.RandomMinMax(15, 55);
        MaxDamage = Utility.RandomMinMax(55, 95);
        Attributes.WeaponSpeed = 5;
        Attributes.BonusDex = 15;
        Slayer = SlayerName.ArachnidDoom;
        WeaponAttributes.HitPoisonArea = 30;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DeadMansLegacy(Serial serial) : base(serial)
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
