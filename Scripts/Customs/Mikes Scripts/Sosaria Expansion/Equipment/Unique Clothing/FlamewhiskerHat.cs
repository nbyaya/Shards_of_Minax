using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FlamewhiskerHat : ChefsToque
{
    [Constructable]
    public FlamewhiskerHat()
    {
        Name = "Flamewhisker Hat";
        Hue = 1153; // Light Red, to represent the "flame" theme

        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusInt = 15;
        Attributes.BonusHits = 10;
        Attributes.RegenMana = 2;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Fire = 15;
        Resistances.Poison = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Cooking, 20.0); // Major skill for a chef's toque
        SkillBonuses.SetValues(1, SkillName.Alchemy, 15.0); // For the mix of spices and potions
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 10.0); // As chefs are often linked to animals

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FlamewhiskerHat(Serial serial) : base(serial)
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
