using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KatanaOfTheEclipse : Katana
{
    [Constructable]
    public KatanaOfTheEclipse()
    {
        Name = "Katana of the Eclipse";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(55, 95);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusHits = 20;
        Attributes.WeaponDamage = 40;
        Attributes.NightSight = 1;
        Slayer = SlayerName.WaterDissipation;
        WeaponAttributes.HitCurse = 30;
        WeaponAttributes.HitLowerAttack = 25;
        SkillBonuses.SetValues(0, SkillName.Bushido, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KatanaOfTheEclipse(Serial serial) : base(serial)
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
