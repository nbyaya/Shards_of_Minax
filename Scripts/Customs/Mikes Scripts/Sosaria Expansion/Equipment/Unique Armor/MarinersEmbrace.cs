using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MarinersEmbrace : DragonTurtleHideBustier
{
    [Constructable]
    public MarinersEmbrace()
    {
        Name = "Mariner's Embrace";
        Hue = 1150;  // Set to a color that gives a mystic or aquatic feel
        BaseArmorRating = 30; // The base armor rating can be balanced for this item

        Attributes.DefendChance = 10;  // Increased defense for survivability in combat
        Attributes.LowerManaCost = 5;  // Reduced mana cost, perfect for mages or casters
        Attributes.BonusStr = 10;  // Bonus strength for better combat performance
        Attributes.BonusDex = 10;  // Bonus dexterity for agility and movement, useful in water

        SkillBonuses.SetValues(0, SkillName.Fishing, 15.0);  // Bonus to fishing skill (thematic for mariners)
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);  // Bonus to Animal Lore (handling sea creatures)
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);  // Bonus to tactics for better combat strategies

        PhysicalBonus = 5;  // Protection against physical damage
        EnergyBonus = 5;    // Protection against energy-based damage, tying in with elemental resistance
        FireBonus = 5;      // Protection against fire-based attacks
        ColdBonus = 5;      // Protection against cold, reflecting the marine environment's challenges

        XmlAttach.AttachTo(this, new XmlLevelItem());  // Attach the custom level item for the unique armor
    }

    public MarinersEmbrace(Serial serial) : base(serial)
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
