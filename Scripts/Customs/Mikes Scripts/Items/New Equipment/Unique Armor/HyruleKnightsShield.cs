using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HyruleKnightsShield : HeaterShield
{
    [Constructable]
    public HyruleKnightsShield()
    {
        Name = "Hyrule Knight's Shield";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterKinetic = 20;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.DefendChance = 15;
        Attributes.ReflectPhysical = 10;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 15;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HyruleKnightsShield(Serial serial) : base(serial)
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
