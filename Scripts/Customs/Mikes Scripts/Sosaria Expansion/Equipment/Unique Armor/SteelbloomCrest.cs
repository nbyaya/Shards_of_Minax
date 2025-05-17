using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SteelbloomCrest : PlateHatsuburi
{
    [Constructable]
    public SteelbloomCrest()
    {
        Name = "Steelbloom Crest";
        Hue = Utility.Random(500, 1100); // Gives a distinctive hue, like tarnished steel
        BaseArmorRating = Utility.RandomMinMax(30, 80); // A solid defensive piece

        // Attributes - Balancing both defense and minor utility
        Attributes.BonusStr = 10;  // Provides strength for combat
        Attributes.BonusDex = 5;   // Offers some dexterity for quicker movement
        Attributes.DefendChance = 5; // Small bonus to defending against attacks
        Attributes.CastSpeed = 1; // Slightly reduced cast speed, providing an edge in magic

        // Skill Bonuses - Focused on combat and strategic defense, fitting the historical nature of the item
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // Increases the effectiveness of combat strategy
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0); // Improved parrying skills for better defense
        SkillBonuses.SetValues(2, SkillName.Swords, 10.0); // Boosts swordsmanship for enhanced combat

        // Elemental Bonuses - Making it resilient against multiple threats
        ColdBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 10; // Emphasizing overall physical protection

        // Attach XmlLevelItem for this unique item
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SteelbloomCrest(Serial serial) : base(serial)
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
