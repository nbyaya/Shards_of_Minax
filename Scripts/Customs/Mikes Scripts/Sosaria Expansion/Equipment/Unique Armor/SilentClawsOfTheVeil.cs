using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SilentClawsOfTheVeil : LeatherNinjaMitts
{
    [Constructable]
    public SilentClawsOfTheVeil()
    {
        Name = "Silent Claws of the Veil";
        Hue = Utility.Random(1150, 1300);  // Dark, stealthy colors.
        BaseArmorRating = Utility.RandomMinMax(5, 20);  // Lightweight armor, designed for stealth.

        // Attributes for Dexterity and Stealth-related enhancements
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.DefendChance = 5; // Increases defense while sneaking or evading attacks.
        Attributes.Luck = 50;  // Adds a little luck for more stealthy, successful actions.
        Attributes.WeaponSpeed = 10;  // Increases attack speed when using ninja tools or weapons.

        // Skill bonuses to enhance stealthy combat and evasion
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 20.0);
        SkillBonuses.SetValues(2, SkillName.Lockpicking, 15.0);

        // Bonus to physical protection from light armor (mainly for avoiding detection).
        PhysicalBonus = 5;
        PoisonBonus = 5;

        // Attach the XML level item for integration into your server’s item leveling system
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SilentClawsOfTheVeil(Serial serial) : base(serial)
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
