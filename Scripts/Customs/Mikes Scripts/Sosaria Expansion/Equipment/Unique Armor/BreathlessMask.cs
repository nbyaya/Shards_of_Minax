using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BreathlessMask : PlateMempo
{
    [Constructable]
    public BreathlessMask()
    {
        Name = "The Breathless Mask";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(25, 50);

        // Attributes
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 5;
        Attributes.DefendChance = 10;
        Attributes.NightSight = 1;
        Attributes.Luck = 50;
        Attributes.WeaponSpeed = 5;

        // Skills
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 20.0);
        SkillBonuses.SetValues(2, SkillName.Anatomy, 15.0);

        // Bonuses for evasion and subtlety
        PhysicalBonus = 10;
        EnergyBonus = 5;

        // Add the XmlLevelItem for progression
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BreathlessMask(Serial serial) : base(serial)
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
