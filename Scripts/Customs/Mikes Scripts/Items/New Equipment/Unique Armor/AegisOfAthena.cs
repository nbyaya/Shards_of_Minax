using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AegisOfAthena : BronzeShield
{
    [Constructable]
    public AegisOfAthena()
    {
        Name = "Aegis of Athena";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.ResonanceEnergy = 25;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.ReflectPhysical = 15;
        Attributes.BonusMana = 50;
        SkillBonuses.SetValues(0, SkillName.Parry, 25.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 20.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AegisOfAthena(Serial serial) : base(serial)
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
