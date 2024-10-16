using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WisdomsCirclet : PlateHelm
{
    [Constructable]
    public WisdomsCirclet()
    {
        Name = "Wisdom's Circlet";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.CastingFocus = 30;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.BonusInt = 40;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WisdomsCirclet(Serial serial) : base(serial)
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
