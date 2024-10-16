using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Thunderstroke : ShortSpear
{
    [Constructable]
    public Thunderstroke()
    {
        Name = "Thunderstroke";
        Hue = Utility.Random(500, 2900);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.IncreasedKarmaLoss = 5;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitLightning = 40;
        WeaponAttributes.HitMagicArrow = 20;
        SkillBonuses.SetValues(0, SkillName.Throwing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Thunderstroke(Serial serial) : base(serial)
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
