using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DesertViperScimitar : Scimitar
{
    [Constructable]
    public DesertViperScimitar()
    {
        Name = "Desert Viper Scimitar";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 95);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusHits = 20;
        Attributes.BonusStam = 25;
        Slayer = SlayerName.ReptilianDeath;
        WeaponAttributes.HitPoisonArea = 50;
        WeaponAttributes.HitFatigue = 30;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DesertViperScimitar(Serial serial) : base(serial)
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
