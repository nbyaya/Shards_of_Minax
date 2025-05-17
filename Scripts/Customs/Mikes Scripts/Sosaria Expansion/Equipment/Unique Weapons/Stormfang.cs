using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Stormfang : ShortSpear
{
    [Constructable]
    public Stormfang()
    {
        Name = "Stormfang";
        Hue = Utility.Random(1150, 1190);  // A shimmering hue of silver and blue, representing the tempest
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 75);
        Attributes.WeaponSpeed = 10;  // Enhanced speed to match the swiftness of a storm
        Attributes.Luck = 15;  // Increase chance of good fortune in battle
        Attributes.DefendChance = 10;  // Slight defense bonus, representing the storm's unpredictable nature
        
        // Slayer effect – Stormfang is especially effective against Serpents, commonly linked to ancient storms and elemental power
        Slayer = SlayerName.SnakesBane;

        // Weapon attributes – Enhanced combat capabilities in the midst of stormy chaos
        WeaponAttributes.HitLightning = 40;  // Adds lightning strikes to hits, perfect for a storm-themed weapon
        WeaponAttributes.HitColdArea = 20;   // Add a chance to hit cold damage to enemies within range, creating chilling storms
        WeaponAttributes.HitFireball = 25;   // Adds fireball effect, invoking fiery explosions in the storm's wake

        // Skill bonuses to enhance combat style and elemental proficiency
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0);  // Enhances the ability to track enemies or dangers
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);  // Increases effectiveness against vulnerable opponents, like how storms target weak points

        // Attach the XML Level Item for further gameplay integration
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Stormfang(Serial serial) : base(serial)
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
