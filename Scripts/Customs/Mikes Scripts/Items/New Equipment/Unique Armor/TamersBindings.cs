using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TamersBindings : LeatherArms
{
    [Constructable]
    public TamersBindings()
    {
        Name = "Tamer's Bindings";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(55, 70);
        AbsorptionAttributes.EaterEnergy = 25;
        ArmorAttributes.LowerStatReq = 30;
        Attributes.BonusDex = 20;
        SkillBonuses.SetValues(0, SkillName.Veterinary, 25.0);
        ColdBonus = 15;
        EnergyBonus = 25;
        FireBonus = 15;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TamersBindings(Serial serial) : base(serial)
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
