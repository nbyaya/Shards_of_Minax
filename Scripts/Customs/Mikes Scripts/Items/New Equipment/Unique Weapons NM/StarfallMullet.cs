using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarfallMullet : WarMace
{
    [Constructable]
    public StarfallMullet()
    {
        Name = "Starfall Mallet";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 40;
        Attributes.BonusHits = 25;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitLightning = 45;
        WeaponAttributes.HitColdArea = 35;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarfallMullet(Serial serial) : base(serial)
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
