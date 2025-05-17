using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SageWrappedLimbs : WoodlandArms
{
    [Constructable]
    public SageWrappedLimbs()
    {
        Name = "Sage-Wrapped Limbs";
        Hue = 0x9C4; // A soft, earthy green hue to represent the Woodland and nature themes.
        BaseArmorRating = Utility.RandomMinMax(15, 40); // Slightly lower base armor rating to fit a lighter, nature-themed item.

        // Attributes reflecting the connection to nature, wisdom, and vitality.
        Attributes.BonusInt = 10;
        Attributes.BonusDex = 15;
        Attributes.BonusHits = 20;
        Attributes.RegenHits = 3;
        Attributes.RegenStam = 3;

        // Skill bonuses that are thematic for the "Woodland" theme and represent the connection with nature, animals, and survival.
        SkillBonuses.SetValues(0, SkillName.Veterinary, 15.0); // Boosts Veterinary skill to aid in animal care and healing.
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0); // Provides better understanding of animals.
        SkillBonuses.SetValues(2, SkillName.Camping, 15.0); // Helps with survival skills and nature-based knowledge.

        // Elemental resistances reflecting nature's resilience.
        ColdBonus = 10;
        PoisonBonus = 15;

        // Special abilities based on nature and wisdom.
        Attributes.Luck = 20; // Grants a slight bonus to luck, drawing on the fortune of the forest.
        Attributes.DefendChance = 5; // Increases defense slightly, representing agility and grace in the wilderness.

        // Attach a level item to this unique armor.
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SageWrappedLimbs(Serial serial) : base(serial)
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
