using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Avalanche : Maul
{
    [Constructable]
    public Avalanche()
    {
        Name = "Avalanche";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 30;
        Attributes.AttackChance = 15;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.Fey;
        WeaponAttributes.HitColdArea = 50;
        WeaponAttributes.HitLightning = 20;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 25.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Avalanche(Serial serial) : base(serial)
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
