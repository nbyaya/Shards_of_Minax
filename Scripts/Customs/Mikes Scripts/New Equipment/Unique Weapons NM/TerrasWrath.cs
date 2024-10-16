using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TerrasWrath : TwoHandedAxe
{
    [Constructable]
    public TerrasWrath()
    {
        Name = "Terra's Wrath";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 20;
        Attributes.BonusHits = 10;
        Slayer = SlayerName.EarthShatter;
        WeaponAttributes.HitFireball = 30;
        WeaponAttributes.HitPhysicalArea = 35;
        SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TerrasWrath(Serial serial) : base(serial)
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
