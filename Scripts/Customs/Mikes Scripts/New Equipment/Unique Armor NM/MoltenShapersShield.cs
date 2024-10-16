using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MoltenShapersShield : BronzeShield
{
    [Constructable]
    public MoltenShapersShield()
    {
        Name = "Molten Shaper's Shield";
        Hue = Utility.Random(600, 700);
        BaseArmorRating = Utility.RandomMinMax(55, 75);
        AbsorptionAttributes.EaterFire = 50;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.DefendChance = 20;
        Attributes.ReflectPhysical = 15;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 50.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 30.0);
        PhysicalBonus = 25;
        FireBonus = 35;
        ColdBonus = 5;
        EnergyBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MoltenShapersShield(Serial serial) : base(serial)
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
