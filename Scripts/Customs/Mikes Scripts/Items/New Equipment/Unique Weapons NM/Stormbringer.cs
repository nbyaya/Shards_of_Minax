using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Stormbringer : BattleAxe
{
    [Constructable]
    public Stormbringer()
    {
        Name = "Stormbringer";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 10;
        Slayer = SlayerName.WaterDissipation;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitEnergyArea = 40;
        WeaponAttributes.HitDispel = 35;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Chivalry, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Stormbringer(Serial serial) : base(serial)
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
