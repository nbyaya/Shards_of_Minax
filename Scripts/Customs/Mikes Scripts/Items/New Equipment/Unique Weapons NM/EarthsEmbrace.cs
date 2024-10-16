using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EarthsEmbrace : Longsword
{
    [Constructable]
    public EarthsEmbrace()
    {
        Name = "Earth's Embrace";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusHits = 40;
        Attributes.DefendChance = 20;
        Slayer = SlayerName.EarthShatter;
        WeaponAttributes.HitHarm = 30;
        WeaponAttributes.HitPhysicalArea = 40;
        SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EarthsEmbrace(Serial serial) : base(serial)
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
