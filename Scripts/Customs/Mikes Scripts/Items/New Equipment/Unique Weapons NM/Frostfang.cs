using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Frostfang : Dagger
{
    [Constructable]
    public Frostfang()
    {
        Name = "Frostfang";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(125, 200);
        Attributes.BonusStam = 20;
        Attributes.WeaponSpeed = 30;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitColdArea = 50;
        WeaponAttributes.HitDispel = 25;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Frostfang(Serial serial) : base(serial)
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
