using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlessingOfTheSilverSun : HolyKnightSword
{
    [Constructable]
    public BlessingOfTheSilverSun()
    {
        Name = "Blessing of the Silver Sun";
        Hue = 1150; // A soft golden hue symbolizing the light of the Silver Sun.
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(70, 100);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 20;
        Attributes.BonusStr = 5; // Adding strength for defensive and offensive balance
        Attributes.BonusDex = 5; // Dexterity aids in speed and agility for combat maneuvers
        Attributes.BonusInt = 5; // Intelligence boosts mana for magical combat support

        // Slayer effect – This weapon excels against those of demonic or undead origin, especially the forces of darkness.
        Slayer = SlayerName.Exorcism; // The weapon is especially effective against dark, undead forces.
        
        // Weapon attributes – Healing and protection are core elements, focusing on preserving life and providing support.
        WeaponAttributes.HitLeechHits = 25; // Restores health with each strike, a hallmark of divine retribution
        WeaponAttributes.HitLeechMana = 15; // Leeching mana in battle is critical for long-lasting fights
        WeaponAttributes.BattleLust = 10; // Provides a boost to combat fervor, encouraging the wielder to engage in righteous battle

        // Skill bonuses for combat and spiritual abilities, aligning with the holy knight’s skill set
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0); // Tactical skill to better wield the sword in battle
        SkillBonuses.SetValues(1, SkillName.Anatomy, 30.0); // Mastery over sword fighting for maximum effectiveness
        SkillBonuses.SetValues(2, SkillName.Chivalry, 15.0); // Amplifies the holy knight's connection to divine powers

        // Thematic addition: Encourages use of divine powers and healing in the midst of battle
        Attributes.RegenHits = 3; // Provides a small but consistent healing factor, a divine blessing
        Attributes.RegenMana = 2; // Restores mana over time to ensure the wielder can cast holy spells when needed

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Ensures the item has the necessary XML attributes for this world’s modding system.
    }

    public BlessingOfTheSilverSun(Serial serial) : base(serial)
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
