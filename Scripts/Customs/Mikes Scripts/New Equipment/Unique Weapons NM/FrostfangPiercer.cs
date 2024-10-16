using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostfangPiercer : Spear
{
    [Constructable]
    public FrostfangPiercer()
    {
        Name = "Frostfang Piercer";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 20;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.ArachnidDoom;
        Slayer2 = SlayerName.FlameDousing;
        WeaponAttributes.HitColdArea = 45;
        WeaponAttributes.HitManaDrain = 35;
        SkillBonuses.SetValues(0, SkillName.Archery, 15.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostfangPiercer(Serial serial) : base(serial)
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
