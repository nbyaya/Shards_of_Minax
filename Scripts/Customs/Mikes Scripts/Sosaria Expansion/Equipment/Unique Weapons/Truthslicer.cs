using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Truthslicer : VivisectionKnife
{
    [Constructable]
    public Truthslicer()
    {
        Name = "Truthslicer";
        Hue = Utility.Random(1150, 1200);  // A sickly green hue, representing the blade's connection to forbidden knowledge and dark arts
        MinDamage = Utility.RandomMinMax(15, 35);
        MaxDamage = Utility.RandomMinMax(40, 60);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;

        // Slayer effect – Truthslicer is particularly effective against creatures that deal with death and the unknown
        Slayer = SlayerName.ArachnidDoom; // Perfect for cutting into the webs of the undead or demonic entities

        // Weapon attributes - enhancing the blade's effectiveness in combat and mystical applications
        WeaponAttributes.HitLeechHits = 25;
        WeaponAttributes.HitLeechMana = 20;
        WeaponAttributes.HitLeechStam = 10;
        WeaponAttributes.HitPoisonArea = 25;
        WeaponAttributes.HitHarm = 15;
        WeaponAttributes.HitDispel = 10;

        // Skill bonuses related to anatomy and the dark art of necromancy
        SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);   // Great for targeting vital organs and weakening the foe
        SkillBonuses.SetValues(1, SkillName.Necromancy, 10.0);  // Enhances the ability to control life and death
        SkillBonuses.SetValues(2, SkillName.Forensics, 15.0);  // Allowing the user to examine and dissect wounds with eerie precision

        // Additional thematic bonuses
        SkillBonuses.SetValues(3, SkillName.Tactics, 5.0);     // Ideal for planning precise strikes with surgical precision
        
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Truthslicer(Serial serial) : base(serial)
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
