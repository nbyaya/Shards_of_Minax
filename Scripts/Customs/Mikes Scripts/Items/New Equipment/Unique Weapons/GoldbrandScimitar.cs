using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GoldbrandScimitar : Scimitar
{
    [Constructable]
    public GoldbrandScimitar()
    {
        Name = "Goldbrand Scimitar";
        Hue = Utility.Random(450, 2500);
        MinDamage = Utility.RandomMinMax(25, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.BonusStr = 10;
        Attributes.RegenStam = 3;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitLightning = 80;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GoldbrandScimitar(Serial serial) : base(serial)
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
