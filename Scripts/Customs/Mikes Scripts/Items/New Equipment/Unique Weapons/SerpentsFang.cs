using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsFang : Kryss
{
    [Constructable]
    public SerpentsFang()
    {
        Name = "Serpent's Fang";
        Hue = Utility.Random(300, 2500);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.BonusDex = 25;
        Attributes.WeaponSpeed = 10;
        Slayer = SlayerName.SnakesBane;
        Slayer2 = SlayerName.Ophidian;
        WeaponAttributes.HitPoisonArea = 50;
        WeaponAttributes.HitLowerAttack = 30;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 25.0);
        SkillBonuses.SetValues(1, SkillName.Fencing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsFang(Serial serial) : base(serial)
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
