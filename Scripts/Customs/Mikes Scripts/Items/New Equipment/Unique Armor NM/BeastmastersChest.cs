using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeastmastersChest : PlateChest
{
    [Constructable]
    public BeastmastersChest()
    {
        Name = "Beastmaster's Chest";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.DurabilityBonus = 100;
        Attributes.BonusStr = 20;
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 50.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 50.0);
        PhysicalBonus = 15;
        FireBonus = 15;
        ColdBonus = 15;
        EnergyBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeastmastersChest(Serial serial) : base(serial)
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
