using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LyricalGreaves : PlateLegs
{
    [Constructable]
    public LyricalGreaves()
    {
        Name = "Lyrical Greaves";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 95);
        AbsorptionAttributes.EaterPoison = 25;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusStam = 40;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 20.0);
        ColdBonus = 15;
        EnergyBonus = 15;
        FireBonus = 20;
        PhysicalBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LyricalGreaves(Serial serial) : base(serial)
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
