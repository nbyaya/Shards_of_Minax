using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarvestersHelm : PlateHelm
{
    [Constructable]
    public HarvestersHelm()
    {
        Name = "Harvester's Helm";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.CastingFocus = 25;
        ArmorAttributes.LowerStatReq = 30;
        Attributes.BonusInt = 25;
        Attributes.Luck = 100;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 25.0);
        ColdBonus = 20;
        EnergyBonus = 20;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarvestersHelm(Serial serial) : base(serial)
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
