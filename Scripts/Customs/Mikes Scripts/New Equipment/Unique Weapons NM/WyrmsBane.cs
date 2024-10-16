using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WyrmsBane : DoubleAxe
{
    [Constructable]
    public WyrmsBane()
    {
        Name = "Wyrm's Bane";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusHits = 50;
        Attributes.Luck = 200;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.ReptilianDeath;
        WeaponAttributes.HitLightning = 35;
        WeaponAttributes.HitManaDrain = 20;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WyrmsBane(Serial serial) : base(serial)
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
