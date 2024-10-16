using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WitchfireShield : WoodenKiteShield
{
    [Constructable]
    public WitchfireShield()
    {
        Name = "Witchfire Shield";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(25, 75);
        AbsorptionAttributes.EaterFire = 30;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.ReflectPhysical = 10;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WitchfireShield(Serial serial) : base(serial)
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
