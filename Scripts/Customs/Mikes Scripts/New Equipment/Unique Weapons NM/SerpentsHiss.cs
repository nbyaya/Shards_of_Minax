using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsHiss : ShortSpear
{
    [Constructable]
    public SerpentsHiss()
    {
        Name = "Serpent's Hiss";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.SnakesBane;
        Slayer2 = SlayerName.ReptilianDeath;
        WeaponAttributes.HitPoisonArea = 50;
        WeaponAttributes.ResistPoisonBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsHiss(Serial serial) : base(serial)
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
