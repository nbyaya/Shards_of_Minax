using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Whisperfang : SilentBlade
{
    [Constructable]
    public Whisperfang()
    {
        Name = "Whisperfang";
        Hue = Utility.Random(1350, 1500);  // A shadowy hue to reflect its stealthy nature
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 75); 
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 20;

        // The Slayer effect – Whisperfang is particularly effective against creatures of the night
        Slayer = SlayerName.ArachnidDoom;  // Effective against creatures like spiders and other night stalkers

        // Weapon attributes – Special focus on stealth and hit effects for a quick, silent strike
        WeaponAttributes.HitLeechHits = 15;
        WeaponAttributes.HitLeechStam = 10;
        WeaponAttributes.HitPoisonArea = 30;
        WeaponAttributes.HitLightning = 10;

        // Skill bonuses in line with stealth and tactical combat, as the blade is a tool of the unseen strike
        SkillBonuses.SetValues(0, SkillName.Swords, 10.0);  // Increase sword skill for better attacks
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);  // Enhance stealth capabilities
        SkillBonuses.SetValues(2, SkillName.Tactics, 5.0);   // Boost tactical advantage in combat

        // Additional thematic bonus for shadowy magic or misdirection
        Attributes.NightSight = 1;  // Allows the wielder to see in the dark, enhancing stealth attacks

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Whisperfang(Serial serial) : base(serial)
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
