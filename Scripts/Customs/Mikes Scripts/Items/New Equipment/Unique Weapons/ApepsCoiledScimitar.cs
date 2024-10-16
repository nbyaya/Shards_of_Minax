using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ApepsCoiledScimitar : Scimitar
{
    [Constructable]
    public ApepsCoiledScimitar()
    {
        Name = "Apep's Coiled Scimitar";
        Hue = Utility.Random(200, 2300);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.ReptilianDeath;
        Slayer2 = SlayerName.Ophidian;
        WeaponAttributes.HitPoisonArea = 30;
        WeaponAttributes.HitManaDrain = 55;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ApepsCoiledScimitar(Serial serial) : base(serial)
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
