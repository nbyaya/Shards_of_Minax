using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CelestialFury : Longsword
{
    [Constructable]
    public CelestialFury()
    {
        Name = "Celestial Fury";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusMana = 30;
        Attributes.CastSpeed = 1;
        Slayer = SlayerName.BloodDrinking;
        Slayer2 = SlayerName.DragonSlaying;
        WeaponAttributes.HitMagicArrow = 35;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CelestialFury(Serial serial) : base(serial)
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
