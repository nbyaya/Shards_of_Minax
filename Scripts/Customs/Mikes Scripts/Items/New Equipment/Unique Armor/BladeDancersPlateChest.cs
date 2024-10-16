using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BladeDancersPlateChest : PlateChest
{
    [Constructable]
    public BladeDancersPlateChest()
    {
        Name = "Blade Dancer's PlateChest";
        Hue = Utility.Random(600, 900);
        BaseArmorRating = Utility.RandomMinMax(50, 85);
        AbsorptionAttributes.EaterDamage = 15;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusStr = 15;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BladeDancersPlateChest(Serial serial) : base(serial)
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
