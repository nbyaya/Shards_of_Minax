using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Keenstrike : Dagger
{
    [Constructable]
    public Keenstrike()
    {
        Name = "Keenstrike";
        Hue = Utility.Random(200, 2400);
        MinDamage = Utility.RandomMinMax(30, 50);
        MaxDamage = Utility.RandomMinMax(50, 90);
        Attributes.AttackChance = 10;
        Attributes.SpellDamage = 15;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitMagicArrow = 30;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Keenstrike(Serial serial) : base(serial)
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
