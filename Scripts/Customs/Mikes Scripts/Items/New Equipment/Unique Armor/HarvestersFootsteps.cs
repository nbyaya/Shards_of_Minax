using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarvestersFootsteps : PlateGorget
{
    [Constructable]
    public HarvestersFootsteps()
    {
        Name = "Harvester's Footsteps";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusMana = 50;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Carpentry, 25.0);
        ColdBonus = 20;
        EnergyBonus = 20;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarvestersFootsteps(Serial serial) : base(serial)
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
