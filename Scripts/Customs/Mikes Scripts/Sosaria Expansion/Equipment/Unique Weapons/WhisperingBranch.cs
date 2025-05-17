using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperingBranch : CompositeBow
{
    [Constructable]
    public WhisperingBranch()
    {
        Name = "Whispering Branch";
        Hue = Utility.Random(1150, 1250);  // A soft green hue representing the bow's connection to nature and the Fey
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 75);
        Attributes.WeaponSpeed = 10;  // Enhances the bow's quickness in combat
        Attributes.Luck = 15;  // Adds some luck, making this bow more desirable for rare drops
        Attributes.ReflectPhysical = 5;  // Provides a small amount of defense in physical combat

        // Slayer effect – This bow is particularly effective against the Fey, aiding in combat against mystical forest creatures
        Slayer = SlayerName.Fey;

        // Weapon attributes
        WeaponAttributes.HitLeechHits = 10;  // This weapon leeches some health from enemies on hit, fitting for a nature-themed weapon
        WeaponAttributes.HitLeechMana = 5;  // Leeching a small amount of mana is useful for keeping the archer supplied in battle
        WeaponAttributes.HitColdArea = 20;  // The bow can release bursts of cold air, freezing enemies in an area

        // Skill bonuses that align with the nature of the bow
        SkillBonuses.SetValues(0, SkillName.Archery, 15.0);  // Boosts the user's skill with bows
        SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);  // Aiding the wielder's stealth, making them more elusive
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 5.0);  // This bonus represents the bond the bow has with nature and creatures
        SkillBonuses.SetValues(3, SkillName.Fletching, 10.0);  // Enhances the ability to craft arrows and further deepen the connection to archery

        // Additional thematic bonus for nature
        SkillBonuses.SetValues(4, SkillName.Mysticism, 5.0);  // The bow’s deep connection to mystical forces enhances the wielder's mysticism

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperingBranch(Serial serial) : base(serial)
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
