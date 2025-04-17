using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CursedArmorCleaver : CooksCleaver
{
    [Constructable]
    public CursedArmorCleaver()
    {
        Name = "Cursed Cleaver";
        Hue = Utility.Random(700, 2900);
        MinDamage = Utility.RandomMinMax(15, 65);
        MaxDamage = Utility.RandomMinMax(65, 105);
        Attributes.BonusHits = -10;
        Attributes.LowerManaCost = 5;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitCurse = 30;
        WeaponAttributes.BloodDrinker = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CursedArmorCleaver(Serial serial) : base(serial)
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
