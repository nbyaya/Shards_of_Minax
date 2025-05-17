using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WispweftRaiment : ElvenShirt
{
    [Constructable]
    public WispweftRaiment()
    {
        Name = "Wispweft Raiment";
        Hue = 1150; // Soft ethereal green, evoking an elven connection to nature

        // Set attributes and bonuses
        Attributes.BonusInt = 8;  // Enhancing magical intelligence
        Attributes.BonusDex = 6;  // Enhancing agility and nimbleness
        Attributes.RegenMana = 3;  // Improving mana regeneration, suitable for spellcasters
        Attributes.LowerManaCost = 5;  // Reducing mana cost for spells, aiding in sustained casting

        // Resistances (light but focused on energy, fitting for a magical, nature-linked item)
        Resistances.Cold = 5;  // Elves are often linked to nature's cool, calm side
        Resistances.Energy = 8;  // Energy resistance for magical protection

        // Skill Bonuses (thematically fitting Elven magic, stealth, and wisdom)
        SkillBonuses.SetValues(0, SkillName.Magery, 10.0);  // Aiding in magical mastery
        SkillBonuses.SetValues(1, SkillName.EvalInt, 8.0);  // Increasing the effectiveness of spells
        SkillBonuses.SetValues(2, SkillName.Stealth, 5.0);  // Elves are known for their quiet movements through nature
        SkillBonuses.SetValues(3, SkillName.AnimalLore, 6.0);  // Elves have a deep connection to the natural world and animals

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WispweftRaiment(Serial serial) : base(serial)
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
