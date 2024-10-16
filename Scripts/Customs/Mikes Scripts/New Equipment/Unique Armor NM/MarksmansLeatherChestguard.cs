using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MarksmansLeatherChestguard : LeatherChest
{
    [Constructable]
    public MarksmansLeatherChestguard()
    {
        Name = "Marksman's Leather Chestguard";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(70, 95);
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusHits = 20;
        Attributes.BonusStam = 15;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Archery, 50.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 30.0);
        SkillBonuses.SetValues(2, SkillName.Healing, 30.0);
        PhysicalBonus = 10;
        EnergyBonus = 10;
        FireBonus = 20;
        ColdBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MarksmansLeatherChestguard(Serial serial) : base(serial)
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
