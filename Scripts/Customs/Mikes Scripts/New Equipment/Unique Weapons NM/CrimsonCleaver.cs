using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CrimsonCleaver : ButcherKnife
{
    [Constructable]
    public CrimsonCleaver()
    {
        Name = "Crimson Cleaver";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 65);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 25;
        Attributes.BonusStam = 10;
        Attributes.WeaponSpeed = 30;
        Slayer = SlayerName.BloodDrinking;
        WeaponAttributes.HitLeechMana = 40;
        WeaponAttributes.BattleLust = 50;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CrimsonCleaver(Serial serial) : base(serial)
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
