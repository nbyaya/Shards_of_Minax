using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Truthrender : DetectivesBoneHarvester
{
    [Constructable]
    public Truthrender()
    {
        Name = "Truthrender";
        Hue = Utility.Random(1300, 1350);  // A dark bone color with subtle hues of shadowy gray, fitting the mysterious nature of the item
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 75);

        // Attribute bonuses related to investigation and forensics
        Attributes.Luck = 15;  // Encourages lucky finds during investigations
        Attributes.BonusDex = 5;  // Dexterity to assist with stealth and quick strikes
        Attributes.BonusInt = 5;  // Increases Intelligence, aiding in solving puzzles and uncovering mysteries
        Attributes.WeaponSpeed = 10;  // Fast strikes for swift investigation

        // Skill bonuses for skills related to investigation and stealth
        SkillBonuses.SetValues(0, SkillName.DetectHidden, 20.0);  // Detecting hidden clues and secret passages
        SkillBonuses.SetValues(1, SkillName.Forensics, 15.0);  // The ability to analyze bodies or clues
        SkillBonuses.SetValues(2, SkillName.Snooping, 20.0);  // Snoop for hidden information
        SkillBonuses.SetValues(3, SkillName.Lockpicking, 10.0);  // Useful for entering restricted areas

        // Slayer effect – given its detective nature, we tie it to enemies who conceal their identities or hide their motives
        Slayer = SlayerName.ArachnidDoom;  // Effective against foes hiding in the shadows, such as assassins or secretive creatures (spiders, assassins, etc.)

        // Additional weapon attributes
        WeaponAttributes.HitLeechHits = 25;  // Leeching health to help the wielder stay in the fight while investigating
        WeaponAttributes.HitLeechMana = 15;  // Leeching mana to empower spellcasting for magical detection
        WeaponAttributes.BattleLust = 10;  // For an added boost in combat when searching for the truth

        XmlAttach.AttachTo(this, new XmlLevelItem());  // Attach the XML level item for persistence and progression
    }

    public Truthrender(Serial serial) : base(serial)
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
