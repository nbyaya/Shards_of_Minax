using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostwardensWoodenShield : WoodenShield
{
    [Constructable]
    public FrostwardensWoodenShield()
    {
        Name = "Frostwarden's WoodenShield";
        Hue = Utility.Random(600, 650);
        BaseArmorRating = Utility.RandomMinMax(43, 73);
        AbsorptionAttributes.ResonanceCold = 25;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.DefendChance = 15;
        Attributes.ReflectPhysical = 10;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        ColdBonus = 25;
        EnergyBonus = 5;
        FireBonus = 0;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostwardensWoodenShield(Serial serial) : base(serial)
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
