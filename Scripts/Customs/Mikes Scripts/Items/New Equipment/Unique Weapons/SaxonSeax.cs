using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SaxonSeax : Dagger
{
    [Constructable]
    public SaxonSeax()
    {
        Name = "Saxon Seax";
        Hue = Utility.Random(100, 2350);
        MinDamage = Utility.RandomMinMax(15, 45);
        MaxDamage = Utility.RandomMinMax(45, 75);
        Attributes.BonusStr = 10;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.ReptilianDeath;
        WeaponAttributes.HitPoisonArea = 20;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SaxonSeax(Serial serial) : base(serial)
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
