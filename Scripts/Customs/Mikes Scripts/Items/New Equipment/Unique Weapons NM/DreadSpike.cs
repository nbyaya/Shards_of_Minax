using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DreadSpike : Kryss
{
    [Constructable]
    public DreadSpike()
    {
        Name = "Dread Spike";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 65);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.WeaponDamage = 35;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.DaemonDismissal;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.HitHarm = 45;
        WeaponAttributes.BattleLust = 25;
        SkillBonuses.SetValues(0, SkillName.Fencing, 25.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DreadSpike(Serial serial) : base(serial)
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
