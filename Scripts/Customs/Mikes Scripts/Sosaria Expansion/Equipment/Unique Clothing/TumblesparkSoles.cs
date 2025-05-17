using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TumblesparkSoles : JesterShoes
{
    [Constructable]
    public TumblesparkSoles()
    {
        Name = "Tumblespark Soles";
        Hue = 1153; // Vibrant, playful color
        Weight = 1.0; // Light to match agility theme
        
        // Set attributes and bonuses
        Attributes.BonusDex = 15; // Boost dexterity for agility and speed
        Attributes.Luck = 50; // Jesters are lucky, or they wouldn’t be able to dodge so well
        Attributes.RegenStam = 2; // Helps maintain energy for quick movement
        
        // Resistances
        Resistances.Physical = 5; // Light resistance to physical attacks
        Resistances.Cold = 5; // Some resilience against cold from the trickster’s will

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0); // Boost stealth to aid in sneaky antics
        SkillBonuses.SetValues(1, SkillName.Snooping, 5.0); // Aid in discovering hidden items or secrets
        SkillBonuses.SetValues(2, SkillName.Stealing, 5.0); // Enhances ability to lift items unnoticed
        
        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TumblesparkSoles(Serial serial) : base(serial)
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
