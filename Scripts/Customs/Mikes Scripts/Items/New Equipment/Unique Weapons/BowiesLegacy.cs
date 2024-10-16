using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BowiesLegacy : ButcherKnife
{
    [Constructable]
    public BowiesLegacy()
    {
        Name = "Bowie's Legacy";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(15, 50);
        MaxDamage = Utility.RandomMinMax(50, 80);
        Attributes.BonusStr = 10;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitLeechHits = 20;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BowiesLegacy(Serial serial) : base(serial)
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
