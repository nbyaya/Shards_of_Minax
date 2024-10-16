using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FishmastersHauberk : ChainChest
{
    [Constructable]
    public FishmastersHauberk()
    {
        Name = "Fishmaster's Hauberk";
        Hue = Utility.Random(50, 150);
        BaseArmorRating = Utility.RandomMinMax(55, 75);
        ArmorAttributes.DurabilityBonus = 100;
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 20;
        Attributes.DefendChance = 20;
        SkillBonuses.SetValues(0, SkillName.Fishing, 50.0);
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 30.0);
        PhysicalBonus = 10;
        EnergyBonus = 15;
        FireBonus = 10;
        ColdBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FishmastersHauberk(Serial serial) : base(serial)
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
