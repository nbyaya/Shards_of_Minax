using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RadiantCrown : PlateHelm
{
    [Constructable]
    public RadiantCrown()
    {
        Name = "Radiant Crown";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterDamage = 30;
        ArmorAttributes.DurabilityBonus = 50;
        Attributes.BonusStr = 20;
        Attributes.RegenHits = 7;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 25.0);
        ColdBonus = 20;
        EnergyBonus = 20;
        FireBonus = 20;
        PhysicalBonus = 25;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RadiantCrown(Serial serial) : base(serial)
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
