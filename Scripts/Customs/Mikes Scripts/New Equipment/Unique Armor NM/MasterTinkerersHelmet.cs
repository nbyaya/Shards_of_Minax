using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MasterTinkerersHelmet : PlateHelm
{
    [Constructable]
    public MasterTinkerersHelmet()
    {
        Name = "Master Tinkerer's Helmet";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        ArmorAttributes.DurabilityBonus = 100;
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusInt = 25;
        Attributes.BonusDex = 15;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 50.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 40.0);
        SkillBonuses.SetValues(2, SkillName.Lockpicking, 40.0);
        PhysicalBonus = 20;
        FireBonus = 15;
        ColdBonus = 15;
        EnergyBonus = 25;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MasterTinkerersHelmet(Serial serial) : base(serial)
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
