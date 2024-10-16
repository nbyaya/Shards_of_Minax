using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Frostbringer : Mace
{
    [Constructable]
    public Frostbringer()
    {
        Name = "Frostbringer";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 20;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.FlameDousing;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.HitColdArea = 45;
        WeaponAttributes.HitFatigue = 30;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Frostbringer(Serial serial) : base(serial)
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
