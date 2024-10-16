using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShadowstrideBow : Bow
{
    [Constructable]
    public ShadowstrideBow()
    {
        Name = "Shadowstride Bow";
        Hue = Utility.Random(600, 2700);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 100);
        Attributes.LowerManaCost = 10;
        Attributes.BonusDex = 10;
        Slayer = SlayerName.ArachnidDoom;
        WeaponAttributes.HitLowerDefend = 20;
        SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
        SkillBonuses.SetValues(1, SkillName.Archery, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShadowstrideBow(Serial serial) : base(serial)
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
