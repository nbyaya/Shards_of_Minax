using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TitansCudgel : Mace
{
    [Constructable]
    public TitansCudgel()
    {
        Name = "The Titan's Cudgel";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 25;
        Attributes.AttackChance = 20;
        Attributes.WeaponDamage = 35;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.EarthShatter;
        WeaponAttributes.HitPhysicalArea = 50;
        WeaponAttributes.DurabilityBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TitansCudgel(Serial serial) : base(serial)
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
