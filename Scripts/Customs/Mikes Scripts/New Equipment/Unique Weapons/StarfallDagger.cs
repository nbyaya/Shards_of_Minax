using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarfallDagger : Dagger
{
    [Constructable]
    public StarfallDagger()
    {
        Name = "Starfall Dagger";
        Hue = Utility.Random(55, 65);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(80, 125);
        Attributes.BonusDex = 25;
        Attributes.AttackChance = 15;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.FlameDousing;
        WeaponAttributes.HitFatigue = 50;
        WeaponAttributes.BattleLust = 30;
        SkillBonuses.SetValues(0, SkillName.Stealth, 30.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarfallDagger(Serial serial) : base(serial)
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
