using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TabulaRasa : StuddedChest
{
    [Constructable]
    public TabulaRasa()
    {
        Name = "Tabula Rasa";
        Hue = Utility.Random(900, 950);
        BaseArmorRating = Utility.RandomMinMax(25, 65);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TabulaRasa(Serial serial) : base(serial)
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
