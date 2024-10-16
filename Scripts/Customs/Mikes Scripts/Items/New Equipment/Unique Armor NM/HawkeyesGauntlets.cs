using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HawkeyesGauntlets : LeatherGloves
{
    [Constructable]
    public HawkeyesGauntlets()
    {
        Name = "Hawkeye's Gauntlets";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(55, 80);
        ArmorAttributes.MageArmor = 1;
        Attributes.AttackChance = 25;
        Attributes.BonusDex = 20;
        SkillBonuses.SetValues(0, SkillName.Archery, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        SkillBonuses.SetValues(2, SkillName.Tracking, 30.0);
        PhysicalBonus = 15;
        ColdBonus = 10;
        FireBonus = 15;
        EnergyBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HawkeyesGauntlets(Serial serial) : base(serial)
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
