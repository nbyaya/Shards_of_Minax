using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SpectralGuardiansChest : PlateChest
{
    [Constructable]
    public SpectralGuardiansChest()
    {
        Name = "Spectral Guardian's Chest";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(50, 75);
        AbsorptionAttributes.EaterEnergy = 25;
        ArmorAttributes.SelfRepair = 10;
        ArmorAttributes.DurabilityBonus = 50;
        Attributes.BonusStr = 15;
        Attributes.BonusInt = 5;
        SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 10.0);
        SkillBonuses.SetValues(1, SkillName.Necromancy, 10.0);
        PhysicalBonus = 5;
        FireBonus = 15;
        ColdBonus = 10;
        EnergyBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SpectralGuardiansChest(Serial serial) : base(serial)
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
