using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DragonsMaw : WarFork
{
    [Constructable]
    public DragonsMaw()
    {
        Name = "Dragon's Maw";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusHits = 30;
        Attributes.BonusStr = 20;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitFireball = 35;
        WeaponAttributes.BattleLust = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DragonsMaw(Serial serial) : base(serial)
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
