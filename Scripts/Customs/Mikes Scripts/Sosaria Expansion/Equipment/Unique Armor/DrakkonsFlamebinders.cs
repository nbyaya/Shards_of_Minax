using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DrakkonsFlamebinders : DragonGloves
{
    [Constructable]
    public DrakkonsFlamebinders()
    {
        Name = "Drakkon's Flamebinders";
        Hue = Utility.Random(1150, 1200); // Red/Flame-colored hue for dragon theme
        BaseArmorRating = Utility.RandomMinMax(25, 60); // Armor rating for gloves

        // Attributes to boost power and combat abilities
        Attributes.BonusStr = 15; // Increases Strength
        Attributes.BonusDex = 10; // Increases Dexterity
        Attributes.BonusInt = 5; // Increases Intelligence
        Attributes.BonusHits = 20; // Boosts HP for survivability
        Attributes.BonusMana = 15; // Increases Mana for fire abilities

        // Combat-related bonuses
        Attributes.DefendChance = 10; // Boosts defense chance
        Attributes.SpellDamage = 8; // Adds extra fire spell damage
        Attributes.LowerManaCost = 5; // Reduces mana cost for fire-related spells
        Attributes.RegenHits = 2; // Gradual HP regen

        // Fire-related elemental bonuses
        FireBonus = 20; // Increases Fire Resistance

        // Skill Bonuses to align with the dragon/fire theme
        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0); // Increase Tactics for better combat strategy
        SkillBonuses.SetValues(1, SkillName.Swords, 15.0); // Boost Swords for combat-focused builds
        SkillBonuses.SetValues(2, SkillName.MagicResist, 10.0); // Increases Magic Resist, useful for fire-based combat

        // Special Flame Binding property: fire-based powers
        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach XmlLevelItem for leveling

        // Unique flavor for item description (optional)
        ItemID = 0x2B6F; // Specific item ID for unique gloves appearance

    }

    public DrakkonsFlamebinders(Serial serial) : base(serial)
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
