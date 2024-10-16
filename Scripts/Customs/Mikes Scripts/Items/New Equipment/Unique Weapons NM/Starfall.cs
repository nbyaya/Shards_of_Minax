using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Starfall : Bow
{
    [Constructable]
    public Starfall()
    {
        Name = "Starfall";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(60, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusMana = 20;
        Attributes.RegenMana = 2;
        Slayer = SlayerName.SummerWind;
        Slayer2 = SlayerName.ElementalBan;
        WeaponAttributes.HitMagicArrow = 55;
        WeaponAttributes.HitDispel = 35;
        SkillBonuses.SetValues(0, SkillName.Archery, 30.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Starfall(Serial serial) : base(serial)
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
