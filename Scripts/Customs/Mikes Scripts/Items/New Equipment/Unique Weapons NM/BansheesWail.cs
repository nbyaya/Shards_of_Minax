using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BansheesWail : ButcherKnife
{
    [Constructable]
    public BansheesWail()
    {
        Name = "Banshee's Wail";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 15;
        Attributes.Luck = 200;
        Attributes.NightSight = 1;
        Slayer = SlayerName.Silver;
        Slayer2 = SlayerName.GargoylesFoe;
        WeaponAttributes.HitHarm = 35;
        WeaponAttributes.HitManaDrain = 25;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BansheesWail(Serial serial) : base(serial)
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
