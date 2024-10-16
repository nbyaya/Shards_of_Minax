using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MelodicCirclet : CloseHelm
{
    [Constructable]
    public MelodicCirclet()
    {
        Name = "Melodic Circlet";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(55, 85);
        AbsorptionAttributes.ResonanceFire = 20;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusInt = 30;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 15.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        ColdBonus = 15;
        EnergyBonus = 15;
        FireBonus = 20;
        PhysicalBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MelodicCirclet(Serial serial) : base(serial)
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
