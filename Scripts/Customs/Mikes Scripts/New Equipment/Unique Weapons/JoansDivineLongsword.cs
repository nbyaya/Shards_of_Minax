using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class JoansDivineLongsword : Longsword
{
    [Constructable]
    public JoansDivineLongsword()
    {
        Name = "Joan's Divine Longsword";
        Hue = Utility.Random(50, 2250);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 100);
        Attributes.SpellChanneling = 1;
        Attributes.BonusHits = 15;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.HitHarm = 30;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public JoansDivineLongsword(Serial serial) : base(serial)
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
