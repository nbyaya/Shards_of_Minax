using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TitansFury : WarHammer
{
    [Constructable]
    public TitansFury()
    {
        Name = "Titan's Fury";
        Hue = Utility.Random(200, 2450);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 140);
        Attributes.BonusHits = 25;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.EarthShatter;
        Slayer2 = SlayerName.ElementalBan;
        WeaponAttributes.HitHarm = 40;
        WeaponAttributes.BattleLust = 60;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 35.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TitansFury(Serial serial) : base(serial)
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
