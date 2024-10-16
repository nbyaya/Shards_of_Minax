using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CrimsonBleaver : WarAxe
{
    [Constructable]
    public CrimsonBleaver()
    {
        Name = "Crimson Cleaver";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 35;
        Attributes.BonusStam = 15;
        Attributes.Luck = 100;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.Ophidian;
        WeaponAttributes.BloodDrinker = 40;
        WeaponAttributes.HitPoisonArea = 30;
        SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CrimsonBleaver(Serial serial) : base(serial)
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
