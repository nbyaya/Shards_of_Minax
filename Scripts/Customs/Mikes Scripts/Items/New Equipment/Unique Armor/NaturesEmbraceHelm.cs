using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NaturesEmbraceHelm : BoneHelm
{
    [Constructable]
    public NaturesEmbraceHelm()
    {
        Name = "Nature's Embrace Helm";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 65);
        AbsorptionAttributes.EaterFire = 20;
        ArmorAttributes.SelfRepair = 5;
        Attributes.RegenHits = 5;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
        SkillBonuses.SetValues(1, SkillName.Herding, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NaturesEmbraceHelm(Serial serial) : base(serial)
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
