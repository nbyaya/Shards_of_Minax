using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BanditsLegs : LeatherLegs
{
    [Constructable]
    public BanditsLegs()
    {
        Name = "Bandit's Legs";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        ArmorAttributes.LowerStatReq = 40;
        Attributes.BonusStam = 25;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealing, 45.0);
        SkillBonuses.SetValues(1, SkillName.Snooping, 50.0);
        SkillBonuses.SetValues(2, SkillName.Lockpicking, 30.0);
        PhysicalBonus = 10;
        EnergyBonus = 25;
        FireBonus = 15;
        ColdBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BanditsLegs(Serial serial) : base(serial)
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
