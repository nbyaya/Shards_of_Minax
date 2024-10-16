using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsFong : Bardiche
{
    [Constructable]
    public SerpentsFong()
    {
        Name = "Serpent's Fang";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 95);
        MaxDamage = Utility.RandomMinMax(205, 250);
        Attributes.BonusStam = 30;
        Attributes.WeaponDamage = 45;
        Slayer = SlayerName.Ophidian;
        Slayer2 = SlayerName.SnakesBane;
        WeaponAttributes.HitPoisonArea = 55;
        WeaponAttributes.HitMagicArrow = 25;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsFong(Serial serial) : base(serial)
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
