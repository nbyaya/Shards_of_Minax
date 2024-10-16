using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsGpite : Spear
{
    [Constructable]
    public SerpentsGpite()
    {
        Name = "Serpent's Spite";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 15;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.SnakesBane;
        Slayer2 = SlayerName.ReptilianDeath;
        WeaponAttributes.HitPoisonArea = 50;
        WeaponAttributes.HitLeechStam = 25;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsGpite(Serial serial) : base(serial)
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
