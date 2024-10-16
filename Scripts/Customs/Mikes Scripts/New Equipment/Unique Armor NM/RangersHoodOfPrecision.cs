using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RangersHoodOfPrecision : LeatherNinjaHood
{
    [Constructable]
    public RangersHoodOfPrecision()
    {
        Name = "Ranger's Hood of Precision";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(60, 85);
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.DurabilityBonus = 100;
        Attributes.AttackChance = 20;
        Attributes.BonusDex = 25;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Archery, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 15;
        EnergyBonus = 15;
        FireBonus = 10;
        ColdBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RangersHoodOfPrecision(Serial serial) : base(serial)
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
