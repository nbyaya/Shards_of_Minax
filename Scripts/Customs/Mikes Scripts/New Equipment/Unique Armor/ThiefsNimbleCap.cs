using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThiefsNimbleCap : LeatherCap
{
    [Constructable]
    public ThiefsNimbleCap()
    {
        Name = "Thief's Nimble Cap";
        Hue = Utility.Random(400, 700);
        BaseArmorRating = Utility.RandomMinMax(15, 50);
        AbsorptionAttributes.EaterEnergy = 10;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusDex = 25;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealing, 15.0);
        ColdBonus = 5;
        EnergyBonus = 15;
        FireBonus = 5;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThiefsNimbleCap(Serial serial) : base(serial)
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
