using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BakersResilientApron : LeatherChest
{
    [Constructable]
    public BakersResilientApron()
    {
        Name = "Baker's Resilient Apron";
        Hue = 1174;
        BaseArmorRating = Utility.RandomMinMax(35, 45);
        ArmorAttributes.MageArmor = 1;
        ArmorAttributes.SelfRepair = 20;
        Attributes.BonusStr = 25;
        Attributes.LowerManaCost = 15;
        Attributes.EnhancePotions = 25;
        SkillBonuses.SetValues(0, SkillName.Cooking, 50.0);
        SkillBonuses.SetValues(1, SkillName.TasteID, 40.0);
        SkillBonuses.SetValues(2, SkillName.ItemID, 30.0);
        PhysicalBonus = 15;
        FireBonus = 5;
        ColdBonus = 20;
        EnergyBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BakersResilientApron(Serial serial) : base(serial)
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
