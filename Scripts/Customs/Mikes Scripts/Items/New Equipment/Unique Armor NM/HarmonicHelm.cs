using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarmonicHelm : ChainCoif
{
    [Constructable]
    public HarmonicHelm()
    {
        Name = "Harmonic Helm";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(40, 55);
        AbsorptionAttributes.EaterCold = 25;
        Attributes.NightSight = 1;
        Attributes.BonusInt = 20;
        Attributes.CastSpeed = 1;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 45.0);
        SkillBonuses.SetValues(1, SkillName.Discordance, 30.0);
        PhysicalBonus = 12;
        FireBonus = 18;
        EnergyBonus = 15;
        ColdBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarmonicHelm(Serial serial) : base(serial)
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
