using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DoomfletchsPrism : Bow
{
    [Constructable]
    public DoomfletchsPrism()
    {
        Name = "Doomfletch's Prism";
        Hue = Utility.Random(700, 2900);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.LowerManaCost = 15;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.ArachnidDoom;
        WeaponAttributes.HitFireArea = 40;
        WeaponAttributes.HitColdArea = 25;
        WeaponAttributes.HitLightning = 25;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DoomfletchsPrism(Serial serial) : base(serial)
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
