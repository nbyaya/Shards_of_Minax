using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GargoylesWane : ShortSpear
{
    [Constructable]
    public GargoylesWane()
    {
        Name = "Gargoyle's Bane";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 95);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 35;
        Attributes.BonusHits = 15;
        Slayer = SlayerName.GargoylesFoe;
        Slayer2 = SlayerName.ElementalBan;
        WeaponAttributes.HitFireball = 40;
        WeaponAttributes.HitDispel = 20;
        SkillBonuses.SetValues(0, SkillName.Macing, 25.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GargoylesWane(Serial serial) : base(serial)
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
