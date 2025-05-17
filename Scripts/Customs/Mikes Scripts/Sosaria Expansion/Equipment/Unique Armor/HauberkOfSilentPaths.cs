using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HauberkOfSilentPaths : ChainChest
{
    [Constructable]
    public HauberkOfSilentPaths()
    {
        Name = "Hauberk of Silent Paths";
        Hue = Utility.Random(2500, 3000); // Choose a dark, stealthy hue
        BaseArmorRating = Utility.RandomMinMax(35, 50); // Solid protection for a chain chest

        // Key attributes to support the stealth theme
        Attributes.BonusDex = 15; // Improves dexterity for agility
        Attributes.BonusStam = 15; // Enhances stamina, allowing more endurance
        Attributes.DefendChance = 10; // Increases defense, helping the wearer evade attacks
        Attributes.LowerManaCost = 5; // Helps those who use magic or abilities
        Attributes.NightSight = 1; // Provides night vision, useful for operating in the dark

        // Skill bonuses tied to stealth and evasion
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0); // Boosts Stealth skill
        SkillBonuses.SetValues(1, SkillName.Hiding, 10.0);  // Boosts Hiding skill
        SkillBonuses.SetValues(2, SkillName.Lockpicking, 10.0);  // Useful for sneaky dungeon explorers

        // Elemental bonuses to add defensive capabilities against all damage types
        ColdBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15; // Increased physical defense since it's chain armor
        PoisonBonus = 10;

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HauberkOfSilentPaths(Serial serial) : base(serial)
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
