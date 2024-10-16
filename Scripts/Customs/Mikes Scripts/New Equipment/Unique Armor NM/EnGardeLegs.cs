using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EnGardeLegs : PlateLegs
{
    [Constructable]
    public EnGardeLegs()
    {
        Name = "En Garde Legs";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(70, 85);
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusStam = 20;
        Attributes.BonusHits = 10;
        SkillBonuses.SetValues(0, SkillName.Fencing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 45.0);
        PhysicalBonus = 15;
        FireBonus = 15;
        ColdBonus = 10;
        EnergyBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EnGardeLegs(Serial serial) : base(serial)
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
