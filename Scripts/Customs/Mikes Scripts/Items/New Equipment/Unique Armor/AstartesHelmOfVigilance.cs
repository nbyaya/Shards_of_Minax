using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AstartesHelmOfVigilance : PlateHelm
{
    [Constructable]
    public AstartesHelmOfVigilance()
    {
        Name = "Astartes Helm of Vigilance";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(27, 87);
        AbsorptionAttributes.EaterEnergy = 20;
        ArmorAttributes.DurabilityBonus = 40;
        Attributes.BonusHits = 20;
        Attributes.NightSight = 1;
        ColdBonus = 15;
        EnergyBonus = 15;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AstartesHelmOfVigilance(Serial serial) : base(serial)
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
