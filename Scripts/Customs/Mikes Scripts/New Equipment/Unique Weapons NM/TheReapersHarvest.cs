using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheReapersHarvest : Bardiche
{
    [Constructable]
    public TheReapersHarvest()
    {
        Name = "The Reaper's Harvest";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(200, 250);
        Attributes.BonusStr = 40;
        Attributes.AttackChance = 25;
        Slayer = SlayerName.Repond;
        WeaponAttributes.HitLeechHits = 50;
        WeaponAttributes.HitHarm = 40;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheReapersHarvest(Serial serial) : base(serial)
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
