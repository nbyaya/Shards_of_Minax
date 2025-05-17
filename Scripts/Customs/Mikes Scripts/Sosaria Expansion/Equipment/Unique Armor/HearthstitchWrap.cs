using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HearthstitchWrap : LeatherSkirt
{
    [Constructable]
    public HearthstitchWrap()
    {
        Name = "Hearthstitch Wrap";
        Hue = Utility.Random(1, 1000); // Random color for variety
        BaseArmorRating = Utility.RandomMinMax(10, 40);

        // Attributes
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusHits = 15;
        Attributes.RegenHits = 2;

        // Specific Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Tailoring, 15.0); // Boost to crafting skills
        SkillBonuses.SetValues(1, SkillName.Cooking, 10.0); // Ties to culinary proficiency and hearth-based themes
        SkillBonuses.SetValues(2, SkillName.Veterinary, 5.0); // Ties to animal care, fitting for the "wrap" theme

        // Elemental Bonuses
        ColdBonus = 5;   // The wrap provides some warmth, especially against cold environments
        PhysicalBonus = 10; // Protective against physical damage from the environment

        // XmlLevelItem for tracking item levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HearthstitchWrap(Serial serial) : base(serial)
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
