using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CrimsonTide : Cutlass
{
    [Constructable]
    public CrimsonTide()
    {
        Name = "Crimson Tide";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(25, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.BonusDex = 10;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.BloodDrinking;
        WeaponAttributes.BattleLust = 50;
        WeaponAttributes.HitPoisonArea = 45;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CrimsonTide(Serial serial) : base(serial)
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
