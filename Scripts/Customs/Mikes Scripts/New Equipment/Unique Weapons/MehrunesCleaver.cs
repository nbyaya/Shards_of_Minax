using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MehrunesCleaver : Cleaver
{
    [Constructable]
    public MehrunesCleaver()
    {
        Name = "Mehrune's Cleaver";
        Hue = Utility.Random(750, 2900);
        MinDamage = Utility.RandomMinMax(15, 45);
        MaxDamage = Utility.RandomMinMax(45, 75);
        Attributes.BonusDex = 10;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.HitLeechStam = 15;
        WeaponAttributes.HitManaDrain = 10;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MehrunesCleaver(Serial serial) : base(serial)
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
