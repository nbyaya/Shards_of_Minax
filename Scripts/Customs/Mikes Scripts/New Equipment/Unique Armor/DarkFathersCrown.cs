using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DarkFathersCrown : PlateHelm
{
    [Constructable]
    public DarkFathersCrown()
    {
        Name = "Dark Father's Crown";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterKinetic = 30;
        ArmorAttributes.DurabilityBonus = 50;
        Attributes.BonusMana = 75;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        ColdBonus = 20;
        EnergyBonus = 20;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DarkFathersCrown(Serial serial) : base(serial)
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
