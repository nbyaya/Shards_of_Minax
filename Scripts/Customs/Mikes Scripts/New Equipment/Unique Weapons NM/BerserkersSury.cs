using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BerserkersSury : VikingSword
{
    [Constructable]
    public BerserkersSury()
    {
        Name = "Berserker's Fury";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(25, 55);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 30;
        Attributes.BonusStam = 10;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.TrollSlaughter;
        Slayer2 = SlayerName.Ophidian;
        WeaponAttributes.HitFireball = 35;
        WeaponAttributes.BattleLust = 50;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BerserkersSury(Serial serial) : base(serial)
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
