using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NightmareMace : WarMace
{
    [Constructable]
    public NightmareMace()
    {
        Name = "Nightmare Mace";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(55, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 30;
        Attributes.ReflectPhysical = 10;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.DaemonDismissal;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitFatigue = 40;
        WeaponAttributes.HitLowerDefend = 30;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NightmareMace(Serial serial) : base(serial)
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
