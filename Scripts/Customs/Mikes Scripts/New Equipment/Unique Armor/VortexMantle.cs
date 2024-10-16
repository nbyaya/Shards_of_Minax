using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VortexMantle : PlateChest
{
    [Constructable]
    public VortexMantle()
    {
        Name = "Vortex Mantle";
        Hue = Utility.Random(1, 500);
        BaseArmorRating = Utility.RandomMinMax(35, 85);
        AbsorptionAttributes.ResonanceEnergy = 50;
        Attributes.LowerManaCost = -15;
        EnergyBonus = -30;
        SkillBonuses.SetValues(0, SkillName.EvalInt, 25.0);
        ColdBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VortexMantle(Serial serial) : base(serial)
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
