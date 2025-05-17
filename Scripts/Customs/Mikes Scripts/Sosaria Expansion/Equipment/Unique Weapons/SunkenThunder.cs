using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SunkenThunder : WarMace
{
    [Constructable]
    public SunkenThunder()
    {
        Name = "Sunken Thunder";
        Hue = Utility.Random(1150, 1200);  // A shimmering golden hue with a faint electric blue glow to represent thunder and ancient energy.
        MinDamage = Utility.RandomMinMax(40, 60);  // Powerful strikes to represent the mace's forceful thunderous blows.
        MaxDamage = Utility.RandomMinMax(70, 100);  // The potential to deal devastating damage with the thunderous impact.
        
        // Adding attributes that emphasize the weapon’s high damage potential and the mystic thunder theme.
        Attributes.WeaponSpeed = 5;  // Ensuring the weapon strikes quickly, like the sudden thunderclap.
        Attributes.Luck = 20;  // The weapon has a sense of destiny, aiding the wielder in battle.
        
        // Slayer effect - Sunken Thunder is especially effective against water-based creatures, perhaps hinting at the sunken origin or ancient, drowned city.
        Slayer = SlayerName.ElementalHealth;
        
        // Weapon attributes - enhancing the weapon’s ability to create disruptive and powerful effects in combat.
        WeaponAttributes.HitLightning = 30;  // A high chance to deal lightning damage, mimicking a thunderstorm’s wrath.
        WeaponAttributes.HitDispel = 25;  // The weapon’s strikes disrupt magical defenses, akin to thunder shattering the calm.

        // Skill bonuses to enhance combat effectiveness and make the weapon more suited to a warrior or tactician.
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);  // Increases the user’s tactical advantage in combat, making them more strategic.
        SkillBonuses.SetValues(1, SkillName.Macing, 20.0);  // Boosts the user’s expertise in using maces.
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);  // Allows the wielder to strike with pinpoint accuracy, targeting weak points.

        // Additional thematic bonus - the weapon draws upon ancient, elemental power, connecting the wielder to the forces of nature.
        SkillBonuses.SetValues(3, SkillName.Focus, 15.0);  // A boost to the wielder’s mental clarity, enabling them to tap into the weapon’s thunderous power more effectively.

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SunkenThunder(Serial serial) : base(serial)
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
