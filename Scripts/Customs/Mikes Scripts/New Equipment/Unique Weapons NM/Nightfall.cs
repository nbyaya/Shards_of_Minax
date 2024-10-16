using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Nightfall : Mace
{
    [Constructable]
    public Nightfall()
    {
        Name = "Nightfall";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(55, 95);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 15;
        Attributes.CastSpeed = 1;
        Attributes.NightSight = 1;
        Slayer = SlayerName.ArachnidDoom;
        Slayer2 = SlayerName.DragonSlaying;
        WeaponAttributes.HitLeechStam = 35;
        WeaponAttributes.HitLightning = 25;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Nightfall(Serial serial) : base(serial)
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
