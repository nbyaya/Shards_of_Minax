using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KnightsAegis : PlateChest
{
    [Constructable]
    public KnightsAegis()
    {
        Name = "Knight's Aegis";
        Hue = Utility.Random(50, 550);
        BaseArmorRating = Utility.RandomMinMax(45, 85);
        AbsorptionAttributes.EaterDamage = 20;
        ArmorAttributes.DurabilityBonus = 30;
        Attributes.BonusStr = 20;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        ColdBonus = 15;
        EnergyBonus = 5;
        FireBonus = 15;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KnightsAegis(Serial serial) : base(serial)
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
