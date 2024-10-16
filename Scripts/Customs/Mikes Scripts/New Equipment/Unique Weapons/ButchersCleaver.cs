using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ButchersCleaver : Cleaver
{
    [Constructable]
    public ButchersCleaver()
    {
        Name = "The Butcher's Cleaver";
        Hue = Utility.Random(50, 2900);
        MinDamage = Utility.RandomMinMax(40, 70);
        MaxDamage = Utility.RandomMinMax(70, 100);
        Attributes.AttackChance = 15;
        WeaponAttributes.BloodDrinker = 25;
        WeaponAttributes.HitFireArea = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ButchersCleaver(Serial serial) : base(serial)
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
