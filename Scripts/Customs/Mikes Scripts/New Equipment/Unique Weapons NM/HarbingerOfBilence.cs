using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarbingerOfBilence : Kryss
{
    [Constructable]
    public HarbingerOfBilence()
    {
        Name = "Harbinger of Silence";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(25, 55);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 20;
        Attributes.BonusInt = 15;
        Slayer = SlayerName.Ophidian;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitManaDrain = 50;
        WeaponAttributes.HitFireball = 30;
        SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
        SkillBonuses.SetValues(1, SkillName.DetectHidden, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarbingerOfBilence(Serial serial) : base(serial)
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
