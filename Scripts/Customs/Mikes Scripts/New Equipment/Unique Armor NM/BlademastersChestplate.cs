using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlademastersChestplate : PlateChest
{
    [Constructable]
    public BlademastersChestplate()
    {
        Name = "Blademaster's Chestplate";
        Hue = Utility.Random(100, 800);
        BaseArmorRating = Utility.RandomMinMax(65, 85);
        ArmorAttributes.DurabilityBonus = 100;
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusStr = 30;
        Attributes.BonusStam = 20;
        Attributes.AttackChance = 15;
        SkillBonuses.SetValues(0, SkillName.Swords, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 40.0);
        PhysicalBonus = 20;
        FireBonus = 15;
        ColdBonus = 15;
        EnergyBonus = 25;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlademastersChestplate(Serial serial) : base(serial)
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
