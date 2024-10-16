using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MysticBowOfLight : Bow
{
    [Constructable]
    public MysticBowOfLight()
    {
        Name = "Mystic Bow of Light";
        Hue = Utility.Random(500, 2700);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.NightSight = 1;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.Exorcism;
        WeaponAttributes.HitLightning = 25;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MysticBowOfLight(Serial serial) : base(serial)
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
