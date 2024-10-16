using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GaeasWrath : Maul
{
    [Constructable]
    public GaeasWrath()
    {
        Name = "Gaea's Wrath";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 35;
        Attributes.RegenHits = 5;
        Attributes.WeaponDamage = 50;
        Slayer = SlayerName.EarthShatter;
        Slayer2 = SlayerName.ElementalHealth;
        WeaponAttributes.HitPoisonArea = 30;
        WeaponAttributes.ResistPoisonBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GaeasWrath(Serial serial) : base(serial)
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
