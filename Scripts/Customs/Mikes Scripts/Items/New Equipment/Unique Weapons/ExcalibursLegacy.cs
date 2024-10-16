using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ExcalibursLegacy : Longsword
{
    [Constructable]
    public ExcalibursLegacy()
    {
        Name = "Excalibur's Legacy";
        Hue = Utility.Random(950, 2900);
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(90, 110);
        Attributes.BonusStr = 10;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLightning = 30;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(1, SkillName.Chivalry, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ExcalibursLegacy(Serial serial) : base(serial)
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
