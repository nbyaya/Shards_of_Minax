using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GargoylesHane : WarFork
{
    [Constructable]
    public GargoylesHane()
    {
        Name = "Gargoyle's Bane";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.GargoylesFoe;
        WeaponAttributes.HitHarm = 50;
        WeaponAttributes.HitMagicArrow = 30;
        SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GargoylesHane(Serial serial) : base(serial)
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
