using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShurikenBracers : LeatherArms
{
    [Constructable]
    public ShurikenBracers()
    {
        Name = "Shuriken Bracers";
        Hue = Utility.Random(500, 600);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        AbsorptionAttributes.EaterFire = 10;
        ArmorAttributes.DurabilityBonus = 10;
        Attributes.AttackChance = 10;
        Attributes.BonusStr = 5;
        SkillBonuses.SetValues(0, SkillName.Archery, 10.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 15;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShurikenBracers(Serial serial) : base(serial)
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
