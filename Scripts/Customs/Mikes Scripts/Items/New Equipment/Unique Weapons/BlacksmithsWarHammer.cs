using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlacksmithsWarHammer : SmithSmasher
{
    [Constructable]
    public BlacksmithsWarHammer()
    {
        Name = "Blacksmith's WarHammer";
        Hue = Utility.Random(400, 2900);
        MinDamage = Utility.RandomMinMax(30, 90);
        MaxDamage = Utility.RandomMinMax(90, 150);
        Attributes.BonusHits = 20;
        Attributes.EnhancePotions = 15;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.DurabilityBonus = 30;
        WeaponAttributes.BloodDrinker = 25;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 25.0);
        SkillBonuses.SetValues(1, SkillName.Mining, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlacksmithsWarHammer(Serial serial) : base(serial)
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
