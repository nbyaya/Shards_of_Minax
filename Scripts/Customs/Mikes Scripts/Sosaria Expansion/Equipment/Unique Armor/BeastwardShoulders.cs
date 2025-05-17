using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeastwardShoulders : HidePauldrons
{
    [Constructable]
    public BeastwardShoulders()
    {
        Name = "Beastward Shoulders";
        Hue = Utility.Random(1, 1000);  // Random hue for visual variety
        BaseArmorRating = Utility.RandomMinMax(18, 45);  // Base armor rating for balanced defense

        // Defensive Attributes
        Attributes.DefendChance = 10;  // Increase defense chance
        Attributes.LowerManaCost = 5;  // Lower mana cost (to represent the connection to nature, using less energy)

        // Animal-Related Attributes
        Attributes.BonusStr = 5;  // Strength bonus to enhance the character’s physical capabilities
        Attributes.BonusDex = 10;  // Dexterity bonus for agile movement, essential for a survivalist nature
        Attributes.BonusHits = 15;  // Bonus hits to boost the wearer’s health and toughness

        // Skill Bonuses related to Animal Lore & Taming
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);  // Increase Animal Lore skill for better handling of animals
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 15.0);  // Bonus to Animal Taming for a deeper bond with beasts
        SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0);  // Bonus to Veterinary skill to aid in healing animals

        // Elemental Resistance
        PhysicalBonus = 15;  // Boost physical resistance for survival in the wild

        // Add the XML attribute for item leveling
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeastwardShoulders(Serial serial) : base(serial)
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
