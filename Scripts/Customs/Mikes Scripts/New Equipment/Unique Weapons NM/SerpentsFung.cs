using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsFung : Halberd
{
    [Constructable]
    public SerpentsFung()
    {
        Name = "Serpent's Fang";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.Luck = 100;
        Slayer = SlayerName.Ophidian;
        Slayer2 = SlayerName.SnakesBane;
        WeaponAttributes.HitPoisonArea = 50;
        WeaponAttributes.HitDispel = 30;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsFung(Serial serial) : base(serial)
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
