using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VialWarden : HeaterShield
{
    [Constructable]
    public VialWarden()
    {
        Name = "Vial Warden";
        Hue = Utility.Random(1, 500);
        BaseArmorRating = Utility.RandomMinMax(60, 85);
        AbsorptionAttributes.EaterCold = 30;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.ReflectPhysical = 10;
        SkillBonuses.SetValues(0, SkillName.Meditation, 25.0);
        ColdBonus = 25;
        EnergyBonus = 15;
        FireBonus = 20;
        PhysicalBonus = 25;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VialWarden(Serial serial) : base(serial)
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
