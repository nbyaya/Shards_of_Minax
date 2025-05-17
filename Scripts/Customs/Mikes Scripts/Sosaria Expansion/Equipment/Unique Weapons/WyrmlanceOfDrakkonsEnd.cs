using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WyrmlanceOfDrakkonsEnd : Lance
{
    [Constructable]
    public WyrmlanceOfDrakkonsEnd()
    {
        Name = "Wyrmlance of Drakkon’s End";
        Hue = 1162;  // Dark red with fiery streaks, representing the fury and death of dragons.
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.WeaponSpeed = 5; // Increase speed to allow for quick thrusts.
        Attributes.Luck = 20;
        Attributes.DefendChance = 10;
        
        // Slayer effect – the lance is especially deadly against dragons and their kin.
        Slayer = SlayerName.DragonSlaying;
        
        // Weapon attributes - thematic bonuses, tied to dragon lore
        WeaponAttributes.HitLeechHits = 25;  // Absorb health on hit, representing the life force of slain dragons.
        WeaponAttributes.HitLeechMana = 15;  // Absorb mana from dragonkin to fuel the wielder’s magic.
        WeaponAttributes.BattleLust = 10;   // Increased damage after each kill, fueled by the rage of Drakkon.
        
        // Skill bonuses to enhance dragon-slaying prowess and combat strategy
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);  // Increase in combat tactics, especially against large creatures.
        SkillBonuses.SetValues(1, SkillName.Swords, 20.0);   // Strengthened swordsmanship for thrusting attacks.
        SkillBonuses.SetValues(2, SkillName.Anatomy, 15.0);  // Increased understanding of dragon anatomy for more effective hits.
        

        Attributes.WeaponDamage = 25; // Bonus to weapon damage, inspired by the power of Drakkon.
        
        // XmlLevelItem to track the item as a unique, special piece of gear
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WyrmlanceOfDrakkonsEnd(Serial serial) : base(serial)
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
