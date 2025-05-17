using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TravelersLacedBreeches : LeatherShorts
{
    [Constructable]
    public TravelersLacedBreeches()
    {
        Name = "Traveler’s Laced Breeches";
        Hue = Utility.Random(1, 1000); // Randomly choose a hue to keep it unique each time
        BaseArmorRating = Utility.RandomMinMax(5, 15); // Moderate protection, fitting for light armor

        Attributes.BonusDex = 10; // Bonus to Dexterity for quick movements
        Attributes.BonusStam = 10; // Boosts stamina to endure long travels
        Attributes.RegenStam = 3; // Helps in regenerating stamina more quickly
        Attributes.Luck = 10; // Gives a slight boost to finding items or treasure during exploration

        SkillBonuses.SetValues(0, SkillName.Camping, 15.0); // Useful for setting up camp during travel
        SkillBonuses.SetValues(1, SkillName.Fishing, 10.0); // Enhances survival in the wilderness
        SkillBonuses.SetValues(2, SkillName.Tinkering, 10.0); // Crafting and making use of found materials on the go

        ColdBonus = 5; // Light protection from cold environments, perfect for varied climates
        PhysicalBonus = 10; // Provides protection from physical damage, but not too heavy for travel

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TravelersLacedBreeches(Serial serial) : base(serial)
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
