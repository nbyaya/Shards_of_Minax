using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TempestsReach : Bow
{
    [Constructable]
    public TempestsReach()
    {
        Name = "Tempest's Reach";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.WeaponSpeed = 25;
        Attributes.BonusHits = 10;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.WaterDissipation;
        WeaponAttributes.HitColdArea = 35;
        WeaponAttributes.HitLightning = 40;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TempestsReach(Serial serial) : base(serial)
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
