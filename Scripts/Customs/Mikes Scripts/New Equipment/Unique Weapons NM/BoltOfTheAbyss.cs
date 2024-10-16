using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BoltOfTheAbyss : Crossbow
{
    [Constructable]
    public BoltOfTheAbyss()
    {
        Name = "Bolt of the Abyss";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusHits = 40;
        Attributes.AttackChance = 25;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.HitLowerAttack = 40;
        WeaponAttributes.HitFireball = 20;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BoltOfTheAbyss(Serial serial) : base(serial)
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
