using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsGang : Kryss
{
    [Constructable]
    public SerpentsGang()
    {
        Name = "Serpent's Fang";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 15;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.SnakesBane;
        WeaponAttributes.HitPoisonArea = 40;
        WeaponAttributes.HitLeechStam = 25;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsGang(Serial serial) : base(serial)
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
