using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MakhairaOfAchilles : VeterinaryLance
{
    [Constructable]
    public MakhairaOfAchilles()
    {
        Name = "Makhaira of Achilles";
        Hue = Utility.Random(400, 2600);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(80, 110);
        Attributes.BonusHits = 20;
        Attributes.BonusDex = 10;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.BloodDrinker = 25;
        WeaponAttributes.HitPhysicalArea = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MakhairaOfAchilles(Serial serial) : base(serial)
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
