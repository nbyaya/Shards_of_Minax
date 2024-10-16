using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsMaul : Club
{
    [Constructable]
    public SerpentsMaul()
    {
        Name = "Serpent's Maul";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.AttackChance = 15;
        Slayer = SlayerName.ReptilianDeath;
        Slayer2 = SlayerName.SnakesBane;
        WeaponAttributes.HitPoisonArea = 45;
        WeaponAttributes.HitLowerDefend = 30;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);
        SkillBonuses.SetValues(1, SkillName.Poisoning, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsMaul(Serial serial) : base(serial)
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
