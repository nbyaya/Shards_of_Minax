using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CourtesansDaintyBuckler : Buckler
{
    [Constructable]
    public CourtesansDaintyBuckler()
    {
        Name = "Courtesan's Dainty Buckler";
        Hue = Utility.Random(100, 500);
        BaseArmorRating = Utility.RandomMinMax(20, 50);
        AbsorptionAttributes.ResonanceCold = 10;
        ArmorAttributes.SelfRepair = 3;
        Attributes.CastRecovery = 1;
        SkillBonuses.SetValues(0, SkillName.Focus, 10.0);
        ColdBonus = 15;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 5;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CourtesansDaintyBuckler(Serial serial) : base(serial)
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
