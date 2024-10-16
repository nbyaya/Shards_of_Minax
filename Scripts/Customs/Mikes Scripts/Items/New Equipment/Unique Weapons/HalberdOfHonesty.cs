using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HalberdOfHonesty : Halberd
{
    [Constructable]
    public HalberdOfHonesty()
    {
        Name = "Halberd of Honesty";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(25, 70);
        MaxDamage = Utility.RandomMinMax(70, 100);
        Attributes.ReflectPhysical = 10;
        Attributes.BonusInt = 15;
        Slayer = SlayerName.Fey;
        WeaponAttributes.HitManaDrain = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HalberdOfHonesty(Serial serial) : base(serial)
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
