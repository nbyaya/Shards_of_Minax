using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Vindicator : DoubleAxe
{
    [Constructable]
    public Vindicator()
    {
        Name = "Vindicator";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 100);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 25;
        Attributes.AttackChance = 20;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.BalronDamnation;
        Slayer2 = SlayerName.DaemonDismissal;
        WeaponAttributes.HitFatigue = 40;
        WeaponAttributes.HitLowerDefend = 35;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Vindicator(Serial serial) : base(serial)
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
