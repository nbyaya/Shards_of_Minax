using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChestplateOfEternalFlame : PlateChest
{
    [Constructable]
    public ChestplateOfEternalFlame()
    {
        Name = "Chestplate of Eternal Flame";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(70, 75);
        ArmorAttributes.MageArmor = 1;
        ArmorAttributes.SelfRepair = 10;
        Attributes.Luck = 100;
        Attributes.BonusStr = 20;
        Attributes.ReflectPhysical = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 50.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 40.0);
        SkillBonuses.SetValues(2, SkillName.Ninjitsu, 30.0);
        PhysicalBonus = 10;
        FireBonus = 25;
        ColdBonus = 10;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChestplateOfEternalFlame(Serial serial) : base(serial)
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
