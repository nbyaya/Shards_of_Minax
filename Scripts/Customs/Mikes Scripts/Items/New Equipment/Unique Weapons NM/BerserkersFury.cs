using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BerserkersFury : DoubleAxe
{
    [Constructable]
    public BerserkersFury()
    {
        Name = "Berserker's Fury";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 30;
        Attributes.BonusStam = 20;
        Attributes.AttackChance = 15;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitFireball = 45;
        WeaponAttributes.BattleLust = 35;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BerserkersFury(Serial serial) : base(serial)
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
