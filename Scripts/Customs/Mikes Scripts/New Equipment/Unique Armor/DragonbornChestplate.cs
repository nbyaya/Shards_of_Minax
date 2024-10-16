using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DragonbornChestplate : PlateChest
{
    [Constructable]
    public DragonbornChestplate()
    {
        Name = "Dragonborn Chestplate";
        Hue = Utility.Random(400, 750);
        BaseArmorRating = Utility.RandomMinMax(45, 85);
        AbsorptionAttributes.EaterFire = 25;
        ArmorAttributes.SelfRepair = 4;
        Attributes.BonusStr = 25;
        Attributes.ReflectPhysical = 10;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        FireBonus = 20;
        EnergyBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        ColdBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DragonbornChestplate(Serial serial) : base(serial)
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
