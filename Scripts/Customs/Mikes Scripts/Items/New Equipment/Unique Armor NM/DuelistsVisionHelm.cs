using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DuelistsVisionHelm : PlateHelm
{
    [Constructable]
    public DuelistsVisionHelm()
    {
        Name = "Duelist's Vision Helm";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(65, 75);
        AbsorptionAttributes.ResonanceKinetic = 20;
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusDex = 30;
        Attributes.RegenStam = 10;
        SkillBonuses.SetValues(0, SkillName.Fencing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 40.0);
        PhysicalBonus = 15;
        ColdBonus = 10;
        FireBonus = 10;
        EnergyBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DuelistsVisionHelm(Serial serial) : base(serial)
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
