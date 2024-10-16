using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GnomesGouge : SkinningKnife
{
    [Constructable]
    public GnomesGouge()
    {
        Name = "Gnome's Gouge";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.BonusHits = 20;
        Slayer = SlayerName.EarthShatter;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.HitFireball = 30;
        WeaponAttributes.HitLowerAttack = 25;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GnomesGouge(Serial serial) : base(serial)
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
