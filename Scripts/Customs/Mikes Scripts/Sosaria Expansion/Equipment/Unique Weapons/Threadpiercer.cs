using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Threadpiercer : SewingNeedle
{
    [Constructable]
    public Threadpiercer()
    {
        Name = "Threadpiercer";
        Hue = Utility.Random(2000, 2300);  // A shadowy hue with silver undertones, fitting for a stealthy weapon
        MinDamage = Utility.RandomMinMax(5, 10);
        MaxDamage = Utility.RandomMinMax(15, 30);
        
        Attributes.WeaponSpeed = 15;  // This needle strikes with unparalleled speed
        Attributes.Luck = 20;  // The fates favor those who wield it
    
        // Slayer effect - Threadpiercer is especially effective against certain mystical enemies, like spirits
        Slayer = SlayerName.ArachnidDoom;  // Could relate to weaving fate and controlling spiders or curses
    
        // Weapon attributes - Each strike may paralyze or harm through a venomous touch
        WeaponAttributes.HitPoisonArea = 30;
        WeaponAttributes.HitLeechStam = 20;  // It drains stamina with a surgical precision, sapping the foe’s strength

        // Skill bonuses to match the nature of a tailor’s precision in combat and assassination
        SkillBonuses.SetValues(0, SkillName.Tailoring, 10.0);  // A boost for tailors using this weapon to craft or fight
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);    // Ideal for stealth-based maneuvers or ambushes
        SkillBonuses.SetValues(2, SkillName.Poisoning, 10.0);  // A subtle poison that deals damage over time
        
        // Additional thematic bonuses
        SkillBonuses.SetValues(3, SkillName.Discordance, 10.0); // The weapon causes a chaotic, disorienting effect
        
        // Unique lore for the weapon
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Threadpiercer(Serial serial) : base(serial)
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
