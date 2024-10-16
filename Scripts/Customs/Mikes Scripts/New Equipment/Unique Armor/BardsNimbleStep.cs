using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BardsNimbleStep : StuddedLegs
{
    [Constructable]
    public BardsNimbleStep()
    {
        Name = "Bard's Nimble Step";
        Hue = Utility.Random(100, 600);
        BaseArmorRating = Utility.RandomMinMax(25, 60);
        AbsorptionAttributes.EaterKinetic = 15;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusDex = 20;
        Attributes.RegenStam = 7;
        SkillBonuses.SetValues(0, SkillName.Discordance, 20.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BardsNimbleStep(Serial serial) : base(serial)
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
