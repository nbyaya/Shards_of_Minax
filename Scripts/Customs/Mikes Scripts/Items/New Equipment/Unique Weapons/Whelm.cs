using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Whelm : WarHammer
{
    [Constructable]
    public Whelm()
    {
        Name = "Whelm";
        Hue = Utility.Random(500, 2600);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.BonusStr = 15;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.HitHarm = 50;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Whelm(Serial serial) : base(serial)
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
