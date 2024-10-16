using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CelestialBeaver : TwoHandedAxe
{
    [Constructable]
    public CelestialBeaver()
    {
        Name = "Celestial Cleaver";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.Luck = 200;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.Fey;
        Slayer2 = SlayerName.Exorcism;
        WeaponAttributes.HitManaDrain = 35;
        WeaponAttributes.HitMagicArrow = 40;
        SkillBonuses.SetValues(0, SkillName.Carpentry, 20.0);
        SkillBonuses.SetValues(1, SkillName.Lumberjacking, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CelestialBeaver(Serial serial) : base(serial)
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
