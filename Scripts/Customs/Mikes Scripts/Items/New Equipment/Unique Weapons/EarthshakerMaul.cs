using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EarthshakerMaul : Maul
{
    [Constructable]
    public EarthshakerMaul()
    {
        Name = "Earthshaker Maul";
        Hue = Utility.Random(100, 2900);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.BonusHits = 15;
        Attributes.AttackChance = 5;
        Slayer = SlayerName.EarthShatter;
        Slayer2 = SlayerName.Terathan;
        WeaponAttributes.HitHarm = 25;
        WeaponAttributes.HitPhysicalArea = 20;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EarthshakerMaul(Serial serial) : base(serial)
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
