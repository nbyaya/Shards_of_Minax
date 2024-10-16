using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class YumiOfEmpressJingu : Bow
{
    [Constructable]
    public YumiOfEmpressJingu()
    {
        Name = "Yumi of Empress Jingu";
        Hue = Utility.Random(100, 2150);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 95);
        Attributes.BonusInt = 15;
        Attributes.LowerRegCost = 10;
        Slayer = SlayerName.Repond;
        WeaponAttributes.HitManaDrain = 20;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public YumiOfEmpressJingu(Serial serial) : base(serial)
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
