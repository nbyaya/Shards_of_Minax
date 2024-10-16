using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EldarRuneGuard : ChainCoif
{
    [Constructable]
    public EldarRuneGuard()
    {
        Name = "Eldar Rune Guard";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.ResonanceKinetic = 20;
        Attributes.LowerRegCost = 15;
        Attributes.BonusMana = 20;
        SkillBonuses.SetValues(0, SkillName.Meditation, 25.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EldarRuneGuard(Serial serial) : base(serial)
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
