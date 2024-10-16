using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ForgeMastersChest : PlateChest
{
    [Constructable]
    public ForgeMastersChest()
    {
        Name = "Forge Master's Chest";
        Hue = Utility.Random(100, 200);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        ArmorAttributes.DurabilityBonus = 100;
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusStr = 25;
        Attributes.BonusStam = 20;
        Attributes.EnhancePotions = 25;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 50.0);
        SkillBonuses.SetValues(1, SkillName.ArmsLore, 30.0);
        PhysicalBonus = 20;
        FireBonus = 15;
        ColdBonus = 10;
        EnergyBonus = 25;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ForgeMastersChest(Serial serial) : base(serial)
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
