using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HammerlordsHelm : PlateHelm
{
    [Constructable]
    public HammerlordsHelm()
    {
        Name = "Hammerlord's Helm";
        Hue = Utility.Random(350, 650);
        BaseArmorRating = Utility.RandomMinMax(40, 80);
        AbsorptionAttributes.EaterKinetic = 10;
        ArmorAttributes.DurabilityBonus = 25;
        Attributes.BonusStr = 10;
        SkillBonuses.SetValues(0, SkillName.Macing, 10.0);
        PhysicalBonus = 15;
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HammerlordsHelm(Serial serial) : base(serial)
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
