using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MagicAxeOfGreatStrength : DoubleAxe
{
    [Constructable]
    public MagicAxeOfGreatStrength()
    {
        Name = "Magic Axe of Great Strength";
        Hue = Utility.Random(100, 2200);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 100);
        Attributes.BonusStr = 25;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.HitMagicArrow = 90;
        SkillBonuses.SetValues(0, SkillName.Macing, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MagicAxeOfGreatStrength(Serial serial) : base(serial)
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
