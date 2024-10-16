using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AxeOfTheJuggernaut : TwoHandedAxe
{
    [Constructable]
    public AxeOfTheJuggernaut()
    {
        Name = "Axe of the Juggernaut";
        Hue = Utility.Random(200, 2400);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(80, 110);
        Attributes.BonusHits = 20;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.EarthShatter;
        WeaponAttributes.HitPhysicalArea = 30;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AxeOfTheJuggernaut(Serial serial) : base(serial)
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
