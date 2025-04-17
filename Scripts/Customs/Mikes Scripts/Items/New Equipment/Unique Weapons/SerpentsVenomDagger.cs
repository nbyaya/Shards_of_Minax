using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsVenomDagger : PoisonBlade
{
    [Constructable]
    public SerpentsVenomDagger()
    {
        Name = "Serpent's Venom Dagger";
        Hue = Utility.Random(500, 2600);
        MinDamage = Utility.RandomMinMax(10, 50);
        MaxDamage = Utility.RandomMinMax(50, 90);
        Attributes.NightSight = 1;
        Attributes.BonusDex = 15;
        Slayer = SlayerName.SnakesBane;
        WeaponAttributes.HitPoisonArea = 40;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsVenomDagger(Serial serial) : base(serial)
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
