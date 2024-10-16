using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostwardensPlateChest : PlateChest
{
    [Constructable]
    public FrostwardensPlateChest()
    {
        Name = "Frostwarden's PlateChest";
        Hue = Utility.Random(600, 650);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        AbsorptionAttributes.ResonanceCold = 20;
        ArmorAttributes.DurabilityBonus = 10;
        Attributes.BonusMana = 15;
        Attributes.CastRecovery = 2;
        SkillBonuses.SetValues(0, SkillName.Meditation, 15.0);
        ColdBonus = 30;
        EnergyBonus = 10;
        FireBonus = 0;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostwardensPlateChest(Serial serial) : base(serial)
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
