using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarvesterOfSorrow : Spear
{
    [Constructable]
    public HarvesterOfSorrow()
    {
        Name = "Harvester of Sorrow";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(55, 95);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 20;
        Attributes.WeaponSpeed = 25;
        Slayer = SlayerName.BloodDrinking;
        Slayer2 = SlayerName.EarthShatter;
        WeaponAttributes.HitHarm = 35;
        WeaponAttributes.BattleLust = 20;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarvesterOfSorrow(Serial serial) : base(serial)
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
