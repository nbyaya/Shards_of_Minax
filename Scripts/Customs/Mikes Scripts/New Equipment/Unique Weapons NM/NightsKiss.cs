using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NightsKiss : SkinningKnife
{
    [Constructable]
    public NightsKiss()
    {
        Name = "Night's Kiss";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 30;
        Attributes.LowerManaCost = 15;
        Slayer = SlayerName.Vacuum;
        WeaponAttributes.HitHarm = 35;
        WeaponAttributes.HitDispel = 20;
        SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NightsKiss(Serial serial) : base(serial)
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
