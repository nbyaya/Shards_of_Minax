using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MagitekInfusedPlate : PlateChest
{
    [Constructable]
    public MagitekInfusedPlate()
    {
        Name = "Magitek Infused Plate";
        Hue = Utility.Random(50, 500);
        BaseArmorRating = Utility.RandomMinMax(50, 85);
        AbsorptionAttributes.EaterEnergy = 25;
        ArmorAttributes.MageArmor = 1;
        Attributes.SpellDamage = 10;
        Attributes.CastRecovery = 2;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 15;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MagitekInfusedPlate(Serial serial) : base(serial)
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
