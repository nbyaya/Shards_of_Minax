using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StormforgedPlateChest : PlateChest
{
    [Constructable]
    public StormforgedPlateChest()
    {
        Name = "Stormforged PlateChest";
        Hue = Utility.Random(550, 850);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        AbsorptionAttributes.ResonanceEnergy = 15;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.CastRecovery = 1;
        Attributes.BonusMana = 20;
        SkillBonuses.SetValues(0, SkillName.EvalInt, 15.0);
        EnergyBonus = 25;
        ColdBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StormforgedPlateChest(Serial serial) : base(serial)
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
