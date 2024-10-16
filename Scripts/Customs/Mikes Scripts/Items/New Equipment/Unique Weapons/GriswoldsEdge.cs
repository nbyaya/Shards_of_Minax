using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GriswoldsEdge : BattleAxe
{
    [Constructable]
    public GriswoldsEdge()
    {
        Name = "Griswold's Edge";
        Hue = Utility.Random(200, 2900);
        MinDamage = Utility.RandomMinMax(30, 65);
        MaxDamage = Utility.RandomMinMax(65, 95);
        Attributes.SpellChanneling = 1;
        Attributes.LowerManaCost = 10;
        WeaponAttributes.MageWeapon = 1;
        WeaponAttributes.HitMagicArrow = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GriswoldsEdge(Serial serial) : base(serial)
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
