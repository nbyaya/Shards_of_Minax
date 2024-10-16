using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HeraldOfDoom : WarHammer
{
    [Constructable]
    public HeraldOfDoom()
    {
        Name = "Herald of Doom";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 35;
        Attributes.LowerManaCost = 15;
        Slayer = SlayerName.BalronDamnation;
        Slayer2 = SlayerName.DaemonDismissal;
        WeaponAttributes.HitHarm = 40;
        WeaponAttributes.HitManaDrain = 45;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HeraldOfDoom(Serial serial) : base(serial)
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
