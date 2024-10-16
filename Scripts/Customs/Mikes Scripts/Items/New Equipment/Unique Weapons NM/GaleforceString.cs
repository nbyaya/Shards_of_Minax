using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GaleforceString : Bow
{
    [Constructable]
    public GaleforceString()
    {
        Name = "Galeforce String";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.AttackChance = 15;
        Slayer = SlayerName.BloodDrinking;
        WeaponAttributes.HitEnergyArea = 50;
        WeaponAttributes.HitLowerAttack = 40;
        SkillBonuses.SetValues(0, SkillName.Archery, 30.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(2, SkillName.Stealth, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GaleforceString(Serial serial) : base(serial)
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
