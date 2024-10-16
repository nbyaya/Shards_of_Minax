using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DespairsShadow : PlateHelm
{
    [Constructable]
    public DespairsShadow()
    {
        Name = "Despair's Shadow";
        Hue = Utility.Random(10, 300);
        BaseArmorRating = Utility.RandomMinMax(35, 75);
        AbsorptionAttributes.EaterEnergy = 20;
        ArmorAttributes.DurabilityBonus = -10;
        Attributes.IncreasedKarmaLoss = 15;
        Attributes.Luck = -45;
        SkillBonuses.SetValues(0, SkillName.Hiding, 10.0);
        ColdBonus = 15;
        EnergyBonus = 20;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DespairsShadow(Serial serial) : base(serial)
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
