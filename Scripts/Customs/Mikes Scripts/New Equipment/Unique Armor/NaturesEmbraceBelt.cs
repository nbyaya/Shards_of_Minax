using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NaturesEmbraceBelt : LeatherGorget
{
    [Constructable]
    public NaturesEmbraceBelt()
    {
        Name = "Nature's Embrace Belt";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(18, 50);
        AbsorptionAttributes.EaterEnergy = 25;
        Attributes.RegenMana = 5;
        Attributes.BonusInt = 30;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 25.0);
        SkillBonuses.SetValues(1, SkillName.Herding, 20.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NaturesEmbraceBelt(Serial serial) : base(serial)
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
