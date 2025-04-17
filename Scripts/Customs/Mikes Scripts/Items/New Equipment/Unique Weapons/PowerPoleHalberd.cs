using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PowerPoleHalberd : TenFootPole
{
    [Constructable]
    public PowerPoleHalberd()
    {
        Name = "Power Pole";
        Hue = Utility.Random(150, 2200);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(70, 100);
        Attributes.BonusStam = 20;
        Attributes.BonusStr = 10;
        Slayer = SlayerName.ReptilianDeath;
        WeaponAttributes.SelfRepair = 3;
        WeaponAttributes.HitHarm = 20;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PowerPoleHalberd(Serial serial) : base(serial)
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
