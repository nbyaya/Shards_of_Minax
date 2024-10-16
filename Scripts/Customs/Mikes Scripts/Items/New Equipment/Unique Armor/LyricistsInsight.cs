using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LyricistsInsight : LeatherCap
{
    [Constructable]
    public LyricistsInsight()
    {
        Name = "Lyricist's Insight";
        Hue = Utility.Random(50, 500);
        BaseArmorRating = Utility.RandomMinMax(15, 50);
        AbsorptionAttributes.EaterFire = 10;
        ArmorAttributes.DurabilityBonus = 10;
        Attributes.BonusMana = 20;
        Attributes.CastRecovery = 1;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 15;
        PhysicalBonus = 5;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LyricistsInsight(Serial serial) : base(serial)
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
