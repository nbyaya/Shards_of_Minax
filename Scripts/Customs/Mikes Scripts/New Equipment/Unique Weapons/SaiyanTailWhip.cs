using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SaiyanTailWhip : Club
{
    [Constructable]
    public SaiyanTailWhip()
    {
        Name = "Saiyan Tail Whip";
        Hue = Utility.Random(50, 2100);
        MinDamage = Utility.RandomMinMax(25, 65);
        MaxDamage = Utility.RandomMinMax(65, 95);
        Attributes.BonusDex = 15;
        Attributes.WeaponSpeed = 5;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.HitFatigue = 25;
        WeaponAttributes.HitPhysicalArea = 80;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SaiyanTailWhip(Serial serial) : base(serial)
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
