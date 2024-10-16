using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperingWind : Bow
{
    [Constructable]
    public WhisperingWind()
    {
        Name = "Whispering Wind";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 40);
        MaxDamage = Utility.RandomMinMax(50, 100);
        Attributes.BonusDex = 30;
        Attributes.DefendChance = 20;
        Slayer = SlayerName.Fey;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitMagicArrow = 50;
        WeaponAttributes.HitLowerAttack = 35;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);
        SkillBonuses.SetValues(2, SkillName.MagicResist, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperingWind(Serial serial) : base(serial)
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
