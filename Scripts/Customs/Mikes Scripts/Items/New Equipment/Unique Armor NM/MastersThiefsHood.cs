using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MastersThiefsHood : LeatherNinjaHood
{
    [Constructable]
    public MastersThiefsHood()
    {
        Name = "Master Thief's Hood";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.DurabilityBonus = 100;
        Attributes.BonusDex = 30;
        Attributes.Luck = 200;
        SkillBonuses.SetValues(0, SkillName.Snooping, 50.0);
        SkillBonuses.SetValues(1, SkillName.Stealing, 50.0);
        SkillBonuses.SetValues(2, SkillName.Stealth, 30.0);
        PhysicalBonus = 15;
        ColdBonus = 10;
        FireBonus = 15;
        EnergyBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MastersThiefsHood(Serial serial) : base(serial)
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
