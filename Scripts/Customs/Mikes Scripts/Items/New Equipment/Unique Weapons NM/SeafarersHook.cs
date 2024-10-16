using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SeafarersHook : Cleaver
{
    [Constructable]
    public SeafarersHook()
    {
        Name = "Seafarer's Hook";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 20;
        Attributes.Luck = 100;
        Slayer = SlayerName.WaterDissipation;
        Slayer2 = SlayerName.SpidersDeath;
        WeaponAttributes.HitFireArea = 40;
        WeaponAttributes.HitPoisonArea = 20;
        SkillBonuses.SetValues(0, SkillName.Fishing, 25.0);
        SkillBonuses.SetValues(1, SkillName.Cooking, 20.0);
        SkillBonuses.SetValues(2, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SeafarersHook(Serial serial) : base(serial)
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
