using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RainwhisperBrim : FloppyHat
{
    [Constructable]
    public RainwhisperBrim()
    {
        Name = "Rainwhisper Brim";
        Hue = 1239; // A muted green to reflect the rain and nature theme
        
        // Set attributes and bonuses
        Attributes.BonusDex = 5; // Increases agility, fitting for navigating through harsh weather
        Attributes.BonusStam = 10; // Increases stamina, useful for travel in adverse conditions
        Attributes.RegenHits = 2; // Healing over time, as the rain aids in recovery
        Attributes.DefendChance = 10; // A small bonus to defense, representing the resilience against the elements
        Attributes.Luck = 50; // Good fortune for surviving in harsh environments

        // Resistances
        Resistances.Cold = 15; // Protection from cold weather conditions, like rain and storms
        Resistances.Poison = 5; // Subtle resistance to toxic environments, like swampy areas or poisonous plants
        Resistances.Energy = 5; // Minor protection against elemental forces

        // Skill Bonuses (thematically chosen for nature and stealth)
        SkillBonuses.SetValues(0, SkillName.Tracking, 10.0); // Enhances ability to track creatures, useful in the wilderness
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0); // Useful for understanding and interacting with wildlife
        SkillBonuses.SetValues(2, SkillName.Stealth, 15.0); // Useful for quietly moving through the wilderness, hidden from sight

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RainwhisperBrim(Serial serial) : base(serial)
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
