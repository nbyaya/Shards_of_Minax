using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RatsNest : LeatherCap
{
    [Constructable]
    public RatsNest()
    {
        Name = "Rat's Nest";
        Hue = Utility.Random(500, 800);
        BaseArmorRating = Utility.RandomMinMax(20, 60);
        Attributes.AttackChance = 20;
        Attributes.BonusDex = 10;
        Attributes.WeaponSpeed = 15;
        Attributes.Luck = 50;
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RatsNest(Serial serial) : base(serial)
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
