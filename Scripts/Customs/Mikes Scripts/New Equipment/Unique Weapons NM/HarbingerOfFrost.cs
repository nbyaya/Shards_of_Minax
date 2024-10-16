using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarbingerOfFrost : ShortSpear
{
    [Constructable]
    public HarbingerOfFrost()
    {
        Name = "Harbinger of Frost";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 100);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 20;
        Attributes.Luck = 100;
        Slayer = SlayerName.FlameDousing;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitColdArea = 55;
        WeaponAttributes.HitLowerDefend = 25;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Camping, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarbingerOfFrost(Serial serial) : base(serial)
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
