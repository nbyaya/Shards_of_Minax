using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CarpentersCrown : WoodenKiteShield
{
    [Constructable]
    public CarpentersCrown()
    {
        Name = "Carpenter's Crown";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(35, 80);
        AbsorptionAttributes.ResonanceKinetic = 15;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusInt = 20;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Carpentry, 30.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CarpentersCrown(Serial serial) : base(serial)
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
