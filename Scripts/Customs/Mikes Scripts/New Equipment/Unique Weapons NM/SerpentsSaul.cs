using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsSaul : Mace
{
    [Constructable]
    public SerpentsSaul()
    {
        Name = "Serpent's Maul";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.BonusDex = 10;
        Attributes.WeaponSpeed = 25;
        Slayer = SlayerName.SnakesBane;
        Slayer2 = SlayerName.ReptilianDeath;
        WeaponAttributes.HitPoisonArea = 40;
        WeaponAttributes.HitDispel = 25;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Poisoning, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsSaul(Serial serial) : base(serial)
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
