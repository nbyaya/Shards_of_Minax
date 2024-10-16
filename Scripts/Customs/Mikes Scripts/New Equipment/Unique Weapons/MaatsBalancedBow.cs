using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MaatsBalancedBow : Bow
{
    [Constructable]
    public MaatsBalancedBow()
    {
        Name = "Ma'at's Balanced Bow";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(15, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.AttackChance = 10;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitLeechStam = 20;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MaatsBalancedBow(Serial serial) : base(serial)
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
