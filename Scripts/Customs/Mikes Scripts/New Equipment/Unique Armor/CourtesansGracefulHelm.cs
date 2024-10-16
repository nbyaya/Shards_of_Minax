using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CourtesansGracefulHelm : PlateHelm
{
    [Constructable]
    public CourtesansGracefulHelm()
    {
        Name = "Courtesan's Graceful Helm";
        Hue = Utility.Random(100, 500);
        BaseArmorRating = Utility.RandomMinMax(25, 55);
        AbsorptionAttributes.EaterFire = 10;
        ArmorAttributes.SelfRepair = 3;
        Attributes.BonusInt = 15;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 10.0);
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 10;
        PhysicalBonus = 5;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CourtesansGracefulHelm(Serial serial) : base(serial)
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
