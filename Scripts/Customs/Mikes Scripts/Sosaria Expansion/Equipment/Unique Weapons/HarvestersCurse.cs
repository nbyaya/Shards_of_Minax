using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarvestersCurse : Pitchfork
{
    [Constructable]
    public HarvestersCurse()
    {
        Name = "Harvester's Curse";
        Hue = Utility.Random(1150, 1250);  // Dark green with hints of deathly black, representing decay and curses.
        MinDamage = Utility.RandomMinMax(15, 40);
        MaxDamage = Utility.RandomMinMax(45, 80);
        Attributes.WeaponSpeed = 5;  // Adds speed to the weapon, reflecting its cursed, swift nature.
        
        // Unique Slayer effect – this pitchfork is especially deadly against creatures connected to death and decay.
        Slayer = SlayerName.ArachnidDoom;  // Effectively against spiders and other pestilent creatures, linking with its "curse" theme.

        // Weapon Attributes: The weapon's curse gives it an eerie ability to siphon life
        WeaponAttributes.HitLeechHits = 30;
        WeaponAttributes.HitLeechMana = 15;
        WeaponAttributes.HitLeechStam = 10;
        
        // Additional tactical bonuses to align with the weapon's dark harvest role
        WeaponAttributes.HitPoisonArea = 50;  // A splash poison effect to represent the spreading of disease.
        WeaponAttributes.HitDispel = 25;  // Dispels magical effects, in line with disrupting curses.

        // Skill Bonuses – Enhancing harvesting and combat-related skills in line with the curse
        SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);  // Effective in understanding and exploiting living creatures' weaknesses.
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);  // Tactical advantage, empowering the weapon’s cursed strikes.
        SkillBonuses.SetValues(2, SkillName.Fishing, 10.0);  // The precision required for handling a cursed weapon like this.
        
        // Adding thematic bonus – the pitchfork is a tool meant for harvesting, both crops and life
        SkillBonuses.SetValues(3, SkillName.Tinkering, 10.0);  // Potentially symbolizing its connection to creating "tools" of life and death.

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarvestersCurse(Serial serial) : base(serial)
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
