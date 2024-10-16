using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FlamebaneWarAxe : WarAxe
{
    [Constructable]
    public FlamebaneWarAxe()
    {
        Name = "Flamebane WarAxe";
        Hue = Utility.Random(400, 2600);
        MinDamage = Utility.RandomMinMax(20, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.BonusStam = 10;
        Attributes.Luck = 100;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitFireArea = 40;
        WeaponAttributes.SelfRepair = 3;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FlamebaneWarAxe(Serial serial) : base(serial)
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
