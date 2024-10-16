using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsCrush : WarMace
{
    [Constructable]
    public SerpentsCrush()
    {
        Name = "Serpent's Crush";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 15;
        Attributes.AttackChance = 20;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.SnakesBane;
        Slayer2 = SlayerName.ReptilianDeath;
        WeaponAttributes.HitPoisonArea = 50;
        WeaponAttributes.HitDispel = 40;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsCrush(Serial serial) : base(serial)
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
