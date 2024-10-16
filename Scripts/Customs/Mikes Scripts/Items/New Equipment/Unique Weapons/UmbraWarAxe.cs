using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class UmbraWarAxe : WarAxe
{
    [Constructable]
    public UmbraWarAxe()
    {
        Name = "Umbra WarAxe";
        Hue = Utility.Random(600, 2650);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.SpellChanneling = 1;
        Attributes.NightSight = 1;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.HitManaDrain = 50;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public UmbraWarAxe(Serial serial) : base(serial)
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
