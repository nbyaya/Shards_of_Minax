using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TempestsWury : WarMace
{
    [Constructable]
    public TempestsWury()
    {
        Name = "Tempest's Fury";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(65, 95);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 35;
        Attributes.WeaponSpeed = 25;
        Attributes.DefendChance = 20;
        Slayer = SlayerName.ElementalBan;
        Slayer2 = SlayerName.FlameDousing;
        WeaponAttributes.HitEnergyArea = 50;
        WeaponAttributes.HitFireArea = 35;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TempestsWury(Serial serial) : base(serial)
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
