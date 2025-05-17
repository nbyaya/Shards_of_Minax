using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NightstepThreads : NinjaTabi
{
    [Constructable]
    public NightstepThreads()
    {
        Name = "Nightstep Threads";
        Hue = 1152;  // Subtle, dark hue fitting a ninja's stealthy attire
        
        // Set attributes and bonuses
        Attributes.BonusDex = 15;  // Increased Dexterity to enhance agility
        Attributes.BonusStam = 10; // Extra stamina for quick movements and stealth
        Attributes.DefendChance = 10;  // Increased defense chance to avoid detection or attacks
        Attributes.LowerManaCost = 5;  // Reduced mana cost, aiding in stealth magic use (e.g., invisibility)
        Attributes.Luck = 20;  // Small luck bonus for finding hidden treasures or evading detection

        // Resistances
        Resistances.Physical = 5; // Minimal physical resistance, reflecting the lightweight nature of the tabi
        Resistances.Fire = 5;  // Minor fire resistance, for those moments of danger
        Resistances.Cold = 5;  // Cold resistance, useful for traversing difficult environments
        Resistances.Poison = 15; // Poison resistance to complement ninja’s antidote usage or stealth poison tactics
        Resistances.Energy = 5; // Minimal resistance to energy, keeping with the nimble nature of the item

        // Skill Bonuses (focused on Stealth, Ninjitsu, and Hiding)
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);  // Boost Stealth skill to enhance sneaky movements
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 15.0);  // Boost Ninjitsu for enhanced shadow abilities
        SkillBonuses.SetValues(2, SkillName.Hiding, 20.0);  // Hiding skill helps with evading enemies and slipping into the shadows
        
        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NightstepThreads(Serial serial) : base(serial)
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
