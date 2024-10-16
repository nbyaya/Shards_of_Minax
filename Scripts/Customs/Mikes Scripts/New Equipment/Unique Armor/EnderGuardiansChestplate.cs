using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EnderGuardiansChestplate : PlateChest
{
    [Constructable]
    public EnderGuardiansChestplate()
    {
        Name = "Ender Guardian's Chestplate";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterEnergy = 30;
        Attributes.ReflectPhysical = 15;
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EnderGuardiansChestplate(Serial serial) : base(serial)
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
