using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GauntletsOfPurity : PlateGloves
{
    [Constructable]
    public GauntletsOfPurity()
    {
        Name = "Gauntlets of Purity";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterPoison = 30;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusDex = 20;
        Attributes.AttackChance = 10;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        ColdBonus = 20;
        EnergyBonus = 20;
        FireBonus = 20;
        PhysicalBonus = 25;
        PoisonBonus = 25;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GauntletsOfPurity(Serial serial) : base(serial)
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
