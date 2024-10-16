using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PhilosophersStonePlateChest : PlateChest
{
    [Constructable]
    public PhilosophersStonePlateChest()
    {
        Name = "Philosopher's Stone PlateChest";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(65, 80);
        ArmorAttributes.DurabilityBonus = 100;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusMana = 40;
        Attributes.LowerManaCost = 20;
        Attributes.LowerRegCost = 20;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 50.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 30.0);
        PhysicalBonus = 15;
        FireBonus = 5;
        ColdBonus = 15;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PhilosophersStonePlateChest(Serial serial) : base(serial)
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
