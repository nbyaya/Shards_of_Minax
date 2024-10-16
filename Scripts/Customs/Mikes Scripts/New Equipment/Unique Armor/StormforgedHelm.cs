using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StormforgedHelm : PlateHelm
{
    [Constructable]
    public StormforgedHelm()
    {
        Name = "Stormforged Helm";
        Hue = Utility.Random(550, 850);
        BaseArmorRating = Utility.RandomMinMax(45, 75);
        AbsorptionAttributes.EaterEnergy = 20;
        ArmorAttributes.SelfRepair = 3;
        Attributes.ReflectPhysical = 10;
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.ItemID, 20.0);
        EnergyBonus = 20;
        ColdBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StormforgedHelm(Serial serial) : base(serial)
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
