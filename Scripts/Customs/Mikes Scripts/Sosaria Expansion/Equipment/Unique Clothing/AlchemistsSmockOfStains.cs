using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AlchemistsSmockOfStains : FullApron
{
    [Constructable]
    public AlchemistsSmockOfStains()
    {
        Name = "Alchemist’s Smock of Stains";
        Hue = 0x48D;  // Dark, earthy color to reflect the nature of an alchemist's work.
        
        // Set attributes and bonuses
        Attributes.BonusInt = 20;
        Attributes.EnhancePotions = 50;  // A strong focus on alchemy and potion-enhancing.
        Attributes.Luck = 50;
        Attributes.LowerManaCost = 10;

        // Resistances
        Resistances.Poison = 20;  // Alchemists are used to handling poisons and toxic substances.
        Resistances.Cold = 5;
        Resistances.Energy = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Alchemy, 15.0);  // Boosts alchemy skill directly.
        SkillBonuses.SetValues(1, SkillName.Cooking, 10.0);  // Reflecting the preparation of potions or mixtures.
        SkillBonuses.SetValues(2, SkillName.Tinkering, 10.0);  // Perhaps tinkering with potion vials or equipment.
        
        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AlchemistsSmockOfStains(Serial serial) : base(serial)
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
