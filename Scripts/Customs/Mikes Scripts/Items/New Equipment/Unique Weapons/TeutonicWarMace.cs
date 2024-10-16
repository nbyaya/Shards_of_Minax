using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TeutonicWarMace : WarMace
{
    [Constructable]
    public TeutonicWarMace()
    {
        Name = "Teutonic WarMace";
        Hue = Utility.Random(200, 2400);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.DefendChance = 15;
        Attributes.BonusHits = 10;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitManaDrain = 20;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TeutonicWarMace(Serial serial) : base(serial)
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
