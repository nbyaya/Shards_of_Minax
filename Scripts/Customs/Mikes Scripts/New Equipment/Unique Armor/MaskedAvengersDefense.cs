using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MaskedAvengersDefense : LeatherChest
{
    [Constructable]
    public MaskedAvengersDefense()
    {
        Name = "Masked Avenger's Defense";
        Hue = Utility.Random(900, 999);
        BaseArmorRating = Utility.RandomMinMax(35, 65);
        Attributes.DefendChance = 15;
        Attributes.BonusHits = 20;
        SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 10;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MaskedAvengersDefense(Serial serial) : base(serial)
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
