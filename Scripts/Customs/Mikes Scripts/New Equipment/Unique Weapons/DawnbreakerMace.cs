using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DawnbreakerMace : Mace
{
    [Constructable]
    public DawnbreakerMace()
    {
        Name = "Dawnbreaker Mace";
        Hue = Utility.Random(500, 600);
        MinDamage = Utility.RandomMinMax(25, 60);
        MaxDamage = Utility.RandomMinMax(60, 95);
        Attributes.ReflectPhysical = 10;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.Exorcism;
        WeaponAttributes.HitFireArea = 30;
        WeaponAttributes.HitDispel = 20;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DawnbreakerMace(Serial serial) : base(serial)
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
