using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostfireCleaver : CooksCleaver
{
    [Constructable]
    public FrostfireCleaver()
    {
        Name = "Frostfire Cleaver";
        Hue = Utility.Random(1, 1000);
        MinDamage = Utility.RandomMinMax(10, 60);
        MaxDamage = Utility.RandomMinMax(60, 110);
        Attributes.BonusStr = 15;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.FlameDousing;
        Slayer2 = SlayerName.WaterDissipation;
        WeaponAttributes.HitFireArea = 30;
        WeaponAttributes.HitColdArea = 30;
        SkillBonuses.SetValues(0, SkillName.Cooking, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostfireCleaver(Serial serial) : base(serial)
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
