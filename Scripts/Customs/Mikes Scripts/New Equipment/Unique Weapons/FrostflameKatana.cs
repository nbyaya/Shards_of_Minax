using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostflameKatana : Katana
{
    [Constructable]
    public FrostflameKatana()
    {
        Name = "Frostflame Katana";
        Hue = Utility.Random(300, 2900);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.BonusStr = 10;
        Attributes.RegenStam = 3;
        Slayer = SlayerName.FlameDousing;
        Slayer2 = SlayerName.WaterDissipation;
        WeaponAttributes.HitFireArea = 30;
        WeaponAttributes.HitColdArea = 30;
        SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostflameKatana(Serial serial) : base(serial)
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
