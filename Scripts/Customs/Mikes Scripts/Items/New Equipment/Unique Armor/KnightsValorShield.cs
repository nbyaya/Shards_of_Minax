using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KnightsValorShield : MetalKiteShield
{
    [Constructable]
    public KnightsValorShield()
    {
        Name = "Knight's Valor Shield";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(40, 85);
        AbsorptionAttributes.EaterKinetic = 30;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusStr = 30;
        Attributes.DefendChance = 20;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KnightsValorShield(Serial serial) : base(serial)
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
