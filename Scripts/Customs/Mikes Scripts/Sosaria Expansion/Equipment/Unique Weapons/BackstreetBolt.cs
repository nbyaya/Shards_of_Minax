using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BackstreetBolt : IllegalCrossbow
{
    [Constructable]
    public BackstreetBolt()
    {
        Name = "Backstreet Bolt";
        Hue = Utility.Random(1150, 1200);  // A dull, dark color representing the underworld
        MinDamage = Utility.RandomMinMax(15, 30);
        MaxDamage = Utility.RandomMinMax(40, 70); 
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;

        // A thematic touch to show this crossbow’s affinity with stealth and subterfuge
        Attributes.BonusDex = 5;
        Attributes.BonusInt = 5;
        
        // Slayer effect – effective against the unsuspecting and unprepared, fitting for a weapon of the underworld
        Slayer = SlayerName.GargoylesFoe;

        // Weapon attributes - designed for stealthy, quick, and quiet strikes
        WeaponAttributes.HitLowerDefend = 20;
        WeaponAttributes.HitLeechHits = 15;
        WeaponAttributes.HitPoisonArea = 25;
        
        // Skill bonuses reflecting mastery in assassination, stealth, and marksmanship
        SkillBonuses.SetValues(0, SkillName.Snooping, 10.0);  // Allows sneaking past enemies
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);   // For improved sneaky movements
        SkillBonuses.SetValues(2, SkillName.Stealing, 20.0);   // Enhancing the crossbow's effectiveness in combat

        // Additional thematic bonus
        SkillBonuses.SetValues(3, SkillName.Lockpicking, 5.0); // Tied to the underground, illicit nature

        // Attach the weapon with an XmlLevelItem to allow for easier use and modification later
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BackstreetBolt(Serial serial) : base(serial)
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
