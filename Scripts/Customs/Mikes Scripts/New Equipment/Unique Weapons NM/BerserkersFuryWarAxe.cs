using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BerserkersFuryWarAxe : WarAxe
{
    [Constructable]
    public BerserkersFuryWarAxe()
    {
        Name = "Berserker's Fury";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 40;
        Attributes.BonusStam = 20;
        Attributes.AttackChance = 25;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.BattleLust = 75;
        WeaponAttributes.HitFireball = 50;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BerserkersFuryWarAxe(Serial serial) : base(serial)
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
