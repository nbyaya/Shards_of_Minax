using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NomadsSunsetGuard : LeatherHaidate
{
    [Constructable]
    public NomadsSunsetGuard()
    {
        Name = "Nomad's Sunset Guard";
        Hue = Utility.Random(2000, 2200); // Sunset-like tones
        BaseArmorRating = Utility.RandomMinMax(10, 40); // Light armor for mobility

        // Attributes for nomadic survival, agility, and mystical connection
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.ReflectPhysical = 5;
        Attributes.Luck = 25;
        Attributes.NightSight = 1;

        // Survival and tracking skill bonuses for the wanderer
        SkillBonuses.SetValues(0, SkillName.Tracking, 15.0);
        SkillBonuses.SetValues(1, SkillName.Camping, 20.0);
        SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0);

        // Elemental resistances for desert survival
        ColdBonus = 5;
        FireBonus = 15;
        PhysicalBonus = 10;

        // Additional bonus for enhanced mobility and stealth
        Attributes.DefendChance = 10;
        Attributes.WeaponSpeed = 5;

        // Attach custom XML properties for dynamic progression
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NomadsSunsetGuard(Serial serial) : base(serial)
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
