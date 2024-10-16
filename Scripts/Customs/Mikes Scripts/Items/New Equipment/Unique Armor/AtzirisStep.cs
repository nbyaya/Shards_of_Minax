using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AtzirisStep : LeatherLegs
{
    [Constructable]
    public AtzirisStep()
    {
        Name = "Atziri's Step";
        Hue = Utility.Random(100, 300);
        BaseArmorRating = Utility.RandomMinMax(20, 60);
        Attributes.DefendChance = 20;
        Attributes.BonusHits = 40;
        Attributes.RegenHits = 5;
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AtzirisStep(Serial serial) : base(serial)
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
