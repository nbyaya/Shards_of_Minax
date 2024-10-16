using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BladedancersOrderShield : OrderShield
{
    [Constructable]
    public BladedancersOrderShield()
    {
        Name = "Blade Dancer's OrderShield";
        Hue = Utility.Random(600, 900);
        BaseArmorRating = Utility.RandomMinMax(35, 70);
        AbsorptionAttributes.EaterPoison = 10;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.Luck = 25;
        Attributes.ReflectPhysical = 15;
        SkillBonuses.SetValues(0, SkillName.Swords, 10.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BladedancersOrderShield(Serial serial) : base(serial)
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
