using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GrappleKingsChestguard : PlateChest
{
    [Constructable]
    public GrappleKingsChestguard()
    {
        Name = "Grapple King's Chestguard";
        Hue = Utility.Random(100, 500);
        BaseArmorRating = Utility.RandomMinMax(70, 90);
        ArmorAttributes.DurabilityBonus = 100;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusStr = 50;
        Attributes.BonusHits = 40;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 50.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 30.0);
        PhysicalBonus = 25;
        FireBonus = 25;
        ColdBonus = 25;
        EnergyBonus = 25;
        PoisonBonus = 25;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GrappleKingsChestguard(Serial serial) : base(serial)
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
