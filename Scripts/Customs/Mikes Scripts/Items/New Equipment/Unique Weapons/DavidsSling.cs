using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DavidsSling : Bow
{
    [Constructable]
    public DavidsSling()
    {
        Name = "David's Sling";
        Hue = Utility.Random(400, 2600);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.Luck = 200;
        Attributes.BonusDex = 10;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitMagicArrow = 45;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DavidsSling(Serial serial) : base(serial)
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
