using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MaskedAvengersAgility : LeatherLegs
{
    [Constructable]
    public MaskedAvengersAgility()
    {
        Name = "Masked Avenger's Agility";
        Hue = Utility.Random(900, 999);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        Attributes.BonusStam = 20;
        Attributes.NightSight = 1;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MaskedAvengersAgility(Serial serial) : base(serial)
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
