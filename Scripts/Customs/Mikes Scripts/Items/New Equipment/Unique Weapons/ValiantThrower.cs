using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ValiantThrower : ButcherKnife
{
    [Constructable]
    public ValiantThrower()
    {
        Name = "Valiant Thrower";
        Hue = Utility.Random(200, 2400);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(50, 90);
        Attributes.BonusDex = 10;
        Attributes.WeaponSpeed = 10;
        Slayer = SlayerName.ReptilianDeath;
        WeaponAttributes.HitLeechHits = 20;
        WeaponAttributes.BloodDrinker = 15;
        SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ValiantThrower(Serial serial) : base(serial)
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
