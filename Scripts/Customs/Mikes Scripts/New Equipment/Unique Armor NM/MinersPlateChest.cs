using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MinersPlateChest : PlateChest
{
    [Constructable]
    public MinersPlateChest()
    {
        Name = "Miner's PlateChest";
        Hue = Utility.Random(400, 500);
        BaseArmorRating = Utility.RandomMinMax(70, 90);
        ArmorAttributes.LowerStatReq = 40;
        ArmorAttributes.DurabilityBonus = 100;
        Attributes.BonusStr = 30;
        Attributes.RegenStam = 5;
        Attributes.Luck = 250;
        SkillBonuses.SetValues(0, SkillName.Mining, 50.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 40.0);
        PhysicalBonus = 20;
        FireBonus = 10;
        ColdBonus = 10;
        EnergyBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MinersPlateChest(Serial serial) : base(serial)
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
