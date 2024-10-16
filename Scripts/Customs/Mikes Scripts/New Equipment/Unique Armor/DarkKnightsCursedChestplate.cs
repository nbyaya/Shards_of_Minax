using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DarkKnightsCursedChestplate : PlateChest
{
    [Constructable]
    public DarkKnightsCursedChestplate()
    {
        Name = "Dark Knight's Cursed Chestplate";
        Hue = Utility.Random(10, 20);
        BaseArmorRating = Utility.RandomMinMax(70, 90);
        AbsorptionAttributes.EaterDamage = 30;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.BonusStr = 50;
        Attributes.Luck = -50;
        SkillBonuses.SetValues(0, SkillName.Parry, 30.0);
        ColdBonus = 25;
        EnergyBonus = 20;
        FireBonus = 30;
        PhysicalBonus = 30;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DarkKnightsCursedChestplate(Serial serial) : base(serial)
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
