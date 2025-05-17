using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HeartwoodCurio : WoodlandChest
{
    [Constructable]
    public HeartwoodCurio()
    {
        Name = "Heartwood Curio";
        Hue = Utility.Random(2200, 2300); // Green, earthy hues to match the woodland theme.
        BaseArmorRating = Utility.RandomMinMax(25, 60); // Balance of defense and mobility.

        // Enhancements for nature-oriented characters
        Attributes.BonusStr = 10; // Strength for survival in the wilderness.
        Attributes.BonusDex = 15; // Dexterity for agile movements.
        Attributes.BonusInt = 10; // Intelligence for nature knowledge and magic.

        Attributes.RegenHits = 3; // Steady health regeneration, like the rejuvenating forces of nature.
        Attributes.DefendChance = 10; // Increase defense to avoid damage from nature's creatures.

        // Skill bonuses that align with nature, animals, and craftsmanship
        SkillBonuses.SetValues(0, SkillName.Veterinary, 20.0); // Boosts your connection to animals.
        SkillBonuses.SetValues(1, SkillName.Carpentry, 15.0); // Boosts woodworking skills.
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 15.0); // Deepens understanding of wildlife.

        // Elemental resistances (defending against the elements of the wild)
        ColdBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 20;

        // Attach the unique identifier to this item
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HeartwoodCurio(Serial serial) : base(serial)
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
