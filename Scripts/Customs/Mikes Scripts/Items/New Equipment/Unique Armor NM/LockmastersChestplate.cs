using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LockmastersChestplate : PlateChest
{
    [Constructable]
    public LockmastersChestplate()
    {
        Name = "Lockmaster's Chestplate";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(70, 90);
        ArmorAttributes.MageArmor = 1;
        ArmorAttributes.SelfRepair = 12;
        Attributes.BonusStr = 20;
        Attributes.AttackChance = 15;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 40.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 30.0);
        SkillBonuses.SetValues(2, SkillName.Lockpicking, 50.0);
        PhysicalBonus = 25;
        FireBonus = 10;
        ColdBonus = 10;
        EnergyBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LockmastersChestplate(Serial serial) : base(serial)
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
