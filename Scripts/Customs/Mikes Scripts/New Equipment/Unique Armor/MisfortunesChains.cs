using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MisfortunesChains : StuddedArms
{
    [Constructable]
    public MisfortunesChains()
    {
        Name = "Misfortune's Chains";
        Hue = Utility.Random(10, 300);
        BaseArmorRating = Utility.RandomMinMax(30, 70);
        AbsorptionAttributes.EaterKinetic = 20;
        ArmorAttributes.LowerStatReq = 5;
        Attributes.IncreasedKarmaLoss = 20;
        Attributes.Luck = -60;
        SkillBonuses.SetValues(0, SkillName.Wrestling, -15.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 10;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MisfortunesChains(Serial serial) : base(serial)
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
