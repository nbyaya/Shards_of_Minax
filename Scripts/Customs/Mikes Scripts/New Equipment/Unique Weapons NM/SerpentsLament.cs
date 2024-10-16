using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsLament : VikingSword
{
    [Constructable]
    public SerpentsLament()
    {
        Name = "Serpent's Lament";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.BonusHits = 20;
        Slayer = SlayerName.ReptilianDeath;
        Slayer2 = SlayerName.LizardmanSlaughter;
        WeaponAttributes.HitPoisonArea = 35;
        WeaponAttributes.HitLowerDefend = 25;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsLament(Serial serial) : base(serial)
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
