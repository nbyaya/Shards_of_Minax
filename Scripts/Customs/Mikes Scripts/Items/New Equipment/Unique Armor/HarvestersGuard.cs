using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarvestersGuard : PlateChest
{
    [Constructable]
    public HarvestersGuard()
    {
        Name = "Harvester's Guard";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        ArmorAttributes.SelfRepair = 15;
        Attributes.BonusHits = 70;
        Attributes.RegenHits = 7;
        SkillBonuses.SetValues(0, SkillName.Mining, 25.0);
        ColdBonus = 20;
        EnergyBonus = 20;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarvestersGuard(Serial serial) : base(serial)
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
