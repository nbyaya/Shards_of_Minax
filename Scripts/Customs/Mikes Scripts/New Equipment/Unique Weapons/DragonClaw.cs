using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DragonClaw : WarFork
{
    [Constructable]
    public DragonClaw()
    {
        Name = "Dragon Claw";
        Hue = Utility.Random(300, 2550);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 100);
        Attributes.BonusDex = 15;
        Attributes.RegenStam = 3;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLeechHits = 20;
        WeaponAttributes.BattleLust = 10;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DragonClaw(Serial serial) : base(serial)
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
