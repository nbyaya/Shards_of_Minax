using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HearthfieldShade : StrawHat
{
    [Constructable]
    public HearthfieldShade()
    {
        Name = "Hearthfield Shade";
        Hue = 0x2D;  // Light greenish color to give it a nature vibe

        // Set attributes and bonuses
        Attributes.Luck = 30;  // Slight luck bonus, perfect for a farming lifestyle
        Attributes.BonusStam = 5;  // Helps the wearer endure longer days in the fields
        Attributes.RegenStam = 3;  // Adds stamina regeneration to symbolize rest in the fields

        // Resistances
        Resistances.Physical = 5;  // Protects a bit from environmental hazards
        Resistances.Cold = 10;  // It's a straw hat, so it provides a bit of warmth for colder days

        // Skill Bonuses (Nature, Crafting, Healing, and Animal-related)
        SkillBonuses.SetValues(0, SkillName.Cooking, 5.0);  // The "hearth" theme ties in well with cooking and providing food
        SkillBonuses.SetValues(1, SkillName.Veterinary, 5.0);  // The caring nature of farming and working with animals
        SkillBonuses.SetValues(2, SkillName.Fishing, 5.0);  // Tying into rural life near water
        SkillBonuses.SetValues(3, SkillName.Herding, 5.0);  // A practical skill for someone in the fields or rural life

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HearthfieldShade(Serial serial) : base(serial)
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
