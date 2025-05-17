using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShadebindWrap : LeatherNinjaHood
{
    [Constructable]
    public ShadebindWrap()
    {
        Name = "Shadebind Wrap";
        Hue = Utility.Random(2200, 2300); // Dark hues fitting a ninja's shadowy presence
        BaseArmorRating = Utility.RandomMinMax(15, 40); // Lower base armor rating for light stealth armor

        // Core attributes: Enhancing the agility and stealth abilities of the wearer
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.LowerManaCost = 10; // Efficient use of mana for ninja-like spells

        // Specific attributes related to stealth, evasion, and ninja skills
        Attributes.DefendChance = 10; // Increase defense chance to avoid hits while stealthy
        Attributes.NightSight = 1; // Ninja often thrive in darkness
        Attributes.WeaponSpeed = 5; // A subtle bonus to attack speed, ideal for swift strikes

        // Skill bonuses that fit the theme of ninjitsu and stealth
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);  // Enhances the ability to move undetected
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 15.0);  // Bonus for using Ninjitsu abilities
        SkillBonuses.SetValues(2, SkillName.Hiding, 10.0);  // Bonus to hide from enemies

        // Elemental bonuses to help with evasion
        ColdBonus = 5;
        FireBonus = 5;

        // Attach a unique level item to the armor
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShadebindWrap(Serial serial) : base(serial)
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
