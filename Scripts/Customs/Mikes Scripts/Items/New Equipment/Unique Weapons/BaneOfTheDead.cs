using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BaneOfTheDead : WarMace
{
    [Constructable]
    public BaneOfTheDead()
    {
        Name = "Bane of the Dead";
        Hue = Utility.Random(600, 2900);
        MinDamage = Utility.RandomMinMax(25, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.SpellChanneling = 1;
        Attributes.NightSight = 1;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.DaemonDismissal;
        WeaponAttributes.HitMagicArrow = 20;
        WeaponAttributes.HitManaDrain = 10;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BaneOfTheDead(Serial serial) : base(serial)
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
