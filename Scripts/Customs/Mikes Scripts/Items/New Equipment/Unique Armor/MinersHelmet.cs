using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MinersHelmet : CloseHelm
{
    [Constructable]
    public MinersHelmet()
    {
        Name = "Miner's Helmet";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterKinetic = 20;
        Attributes.BonusStr = 10;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MinersHelmet(Serial serial) : base(serial)
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
