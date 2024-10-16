using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AlchemistsVisionaryHelm : PlateHelm
{
    [Constructable]
    public AlchemistsVisionaryHelm()
    {
        Name = "Alchemist's Visionary Helm";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(30, 65);
        AbsorptionAttributes.EaterPoison = 20;
        ArmorAttributes.SelfRepair = 4;
        Attributes.BonusInt = 15;
        Attributes.EnhancePotions = 10;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 25.0);
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AlchemistsVisionaryHelm(Serial serial) : base(serial)
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
