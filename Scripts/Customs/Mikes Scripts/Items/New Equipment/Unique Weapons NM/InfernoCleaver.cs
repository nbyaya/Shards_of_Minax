using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class InfernoCleaver : LargeBattleAxe
{
    [Constructable]
    public InfernoCleaver()
    {
        Name = "Inferno Cleaver";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.WeaponDamage = 35;
        Slayer = SlayerName.FlameDousing;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitFireball = 50;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public InfernoCleaver(Serial serial) : base(serial)
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
