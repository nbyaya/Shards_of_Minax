using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarbingerOfSilence : Crossbow
{
    [Constructable]
    public HarbingerOfSilence()
    {
        Name = "Harbinger of Silence";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 30;
        Attributes.WeaponSpeed = 20;
        Slayer = SlayerName.ArachnidDoom;
        Slayer2 = SlayerName.Silver;
        WeaponAttributes.HitManaDrain = 35;
        WeaponAttributes.HitColdArea = 80;
        SkillBonuses.SetValues(0, SkillName.Archery, 30.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarbingerOfSilence(Serial serial) : base(serial)
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
