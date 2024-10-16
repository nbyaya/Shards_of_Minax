using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RedoranDefendersGreaves : PlateLegs
{
    [Constructable]
    public RedoranDefendersGreaves()
    {
        Name = "Redoran Defender's Greaves";
        Hue = Utility.Random(300, 500);
        BaseArmorRating = Utility.RandomMinMax(50, 85);
        AbsorptionAttributes.EaterKinetic = 20;
        ArmorAttributes.LowerStatReq = 10;
        Attributes.BonusHits = 40;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 15;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RedoranDefendersGreaves(Serial serial) : base(serial)
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
