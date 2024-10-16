using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TechnicosTassets : PlateHaidate
{
    [Constructable]
    public TechnicosTassets()
    {
        Name = "Technico's Tassets";
        Hue = Utility.Random(10, 250);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusDex = 40;
        Attributes.RegenStam = 5;
        Attributes.LowerManaCost = 20;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 50.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 35.0);
        PhysicalBonus = 18;
        ColdBonus = 18;
        FireBonus = 18;
        EnergyBonus = 18;
        PoisonBonus = 18;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TechnicosTassets(Serial serial) : base(serial)
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
