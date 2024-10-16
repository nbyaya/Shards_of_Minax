using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DwemerAegis : MetalKiteShield
{
    [Constructable]
    public DwemerAegis()
    {
        Name = "Dwemer Aegis";
        Hue = Utility.Random(200, 700);
        BaseArmorRating = Utility.RandomMinMax(45, 80);
        AbsorptionAttributes.ResonanceEnergy = 15;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.ReflectPhysical = 15;
        Attributes.BonusMana = 20;
        SkillBonuses.SetValues(0, SkillName.Anatomy, 15.0);
        ColdBonus = 15;
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DwemerAegis(Serial serial) : base(serial)
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
