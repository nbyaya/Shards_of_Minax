using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SoftstepsDelight : SnoopersPaddle
{
    [Constructable]
    public SoftstepsDelight()
    {
        Name = "Softstep’s Delight";
        Hue = Utility.Random(1100, 1250);  // A soft blend of earthy tones, perfect for hiding in shadows
        MinDamage = Utility.RandomMinMax(10, 25);
        MaxDamage = Utility.RandomMinMax(30, 50);
        Attributes.WeaponSpeed = 5; // Quick, quiet strikes
        Attributes.Luck = 30; // Ensures better outcomes for stealth-based activities

        // Slayer effect - enhanced for sneaky, low-profile encounters
        Slayer = SlayerName.Silver; // The paddle is effective against creatures with heightened awareness

        // Weapon attributes enhancing the stealth and thievery aspects
        WeaponAttributes.HitLeechHits = 15; // Stealing vitality
        WeaponAttributes.HitLeechMana = 10; // Leeching magical energy silently

        // Skill bonuses to emphasize stealth and thievery-related skills
        SkillBonuses.SetValues(0, SkillName.Snooping, 20.0);  // Enhances the ability to detect hidden objects
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);  // Increases stealth while wielding this weapon
        SkillBonuses.SetValues(2, SkillName.Swords, 10.0);  // A minor boost to combat proficiency
        SkillBonuses.SetValues(3, SkillName.Lockpicking, 10.0);  // Strengthens lockpicking while the paddle is equipped

        // A unique attribute that gives a thematic bonus: a sense of quiet stealth
        Attributes.ReflectPhysical = 5;  // Reflecting physical damage from enemies while sneaking
        Attributes.NightSight = 1;  // This weapon provides the wielder with enhanced night vision in the dark


        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SoftstepsDelight(Serial serial) : base(serial)
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
