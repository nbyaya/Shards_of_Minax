using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NinjaWrappings : LeatherChest
{
    [Constructable]
    public NinjaWrappings()
    {
        Name = "Ninja Wrappings";
        Hue = Utility.Random(500, 600);
        BaseArmorRating = Utility.RandomMinMax(35, 65);
        AbsorptionAttributes.EaterEnergy = 15;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusDex = 15;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Ninjitsu, 20.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NinjaWrappings(Serial serial) : base(serial)
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
