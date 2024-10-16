using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MuramasasBloodlust : Katana
{
    [Constructable]
    public MuramasasBloodlust()
    {
        Name = "Muramasa's Bloodlust";
        Hue = Utility.Random(500, 2550);
        MinDamage = Utility.RandomMinMax(30, 75);
        MaxDamage = Utility.RandomMinMax(75, 110);
        Attributes.AttackChance = 10;
        Attributes.BonusStr = 10;
        Slayer = SlayerName.BloodDrinking;
        WeaponAttributes.BloodDrinker = 20;
        WeaponAttributes.HitLeechHits = 15;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MuramasasBloodlust(Serial serial) : base(serial)
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
