using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CourtesansFlowingRobe : LeatherChest
{
    [Constructable]
    public CourtesansFlowingRobe()
    {
        Name = "Courtesan's Flowing Robe";
        Hue = Utility.Random(100, 500);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        ArmorAttributes.MageArmor = 1;
        Attributes.EnhancePotions = 10;
        Attributes.BonusMana = 15;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 10.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CourtesansFlowingRobe(Serial serial) : base(serial)
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
