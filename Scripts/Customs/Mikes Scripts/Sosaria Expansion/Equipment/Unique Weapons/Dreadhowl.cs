using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Dreadhowl : BattleAxe
{
    [Constructable]
    public Dreadhowl()
    {
        Name = "Dreadhowl";
        Hue = Utility.Random(1250, 1350);  // A dark, blood-red hue to represent the axe's fearsome aura
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(75, 100); 
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 10;
        
        // Slayer effect – Dreadhowl is especially effective against creatures that dwell in fear
        Slayer = SlayerName.ArachnidDoom;  // A fitting slayer for an axe that causes terror, effective against spiders and similar creatures

        // Weapon attributes - amplifying the axe's fearsome impact and brutality
        WeaponAttributes.HitLeechHits = 25;  // A portion of the damage returns as health to the wielder
        WeaponAttributes.HitLeechMana = 15;  // Allows the wielder to regain mana with each blow
        WeaponAttributes.HitLeechStam = 10;  // Helps the wielder regain stamina with every strike
        WeaponAttributes.BattleLust = 20;  // Increases the weapon's effectiveness in combat, especially against fearful enemies

        // Skill bonuses reflecting the weapon's focus on intimidating and dominating foes
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Enhances tactical combat, allowing the wielder to dictate the pace of battle
        SkillBonuses.SetValues(1, SkillName.Lumberjacking, 10.0);  // Adds expertise in weapon handling
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 10.0);  // Grants bonus to unarmed combat, in line with the weapon’s primal nature
        
        // Additional thematic bonus
        SkillBonuses.SetValues(3, SkillName.Swords, 5.0);  // A subtle nod to the wielder’s potential sword-fighting skill, representing versatile battle mastery
        
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Dreadhowl(Serial serial) : base(serial)
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
