using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HelmOfDarkness : Mace
{
    [Constructable]
    public HelmOfDarkness()
    {
        Name = "Mace of Darkness";
        Hue = Utility.Random(100, 2300);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(50, 80);
        Attributes.NightSight = 1;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.HitManaDrain = 20;
        WeaponAttributes.LowerStatReq = 10;
        SkillBonuses.SetValues(0, SkillName.Hiding, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HelmOfDarkness(Serial serial) : base(serial)
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
