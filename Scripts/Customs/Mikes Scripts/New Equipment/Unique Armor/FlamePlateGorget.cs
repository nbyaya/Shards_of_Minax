using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FlamePlateGorget : PlateGorget
{
    [Constructable]
    public FlamePlateGorget()
    {
        Name = "Flame PlateGorget";
        Hue = Utility.Random(500, 600);
        BaseArmorRating = Utility.RandomMinMax(35, 70);
        Attributes.CastSpeed = 1;
        Attributes.SpellDamage = 10;
        FireBonus = 15;
        EnergyBonus = 5;
        PoisonBonus = -5;
        PhysicalBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FlamePlateGorget(Serial serial) : base(serial)
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
