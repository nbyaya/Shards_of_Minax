using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class OceanclaspBands : DragonTurtleHideArms
{
    [Constructable]
    public OceanclaspBands()
    {
        Name = "Oceanclasp Bands";
        Hue = Utility.Random(3000, 3200); // Gives it an oceanic green/blue tint
        BaseArmorRating = Utility.RandomMinMax(25, 55); // Moderate armor value

        // Attributes
        Attributes.BonusStr = 10; // Enhance strength for physical durability
        Attributes.BonusDex = 5;  // Adds dexterity for quickness and agility
        Attributes.DefendChance = 10; // Increased chance to defend against attacks

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Fishing, 15.0); // The oceanic connection enhances fishing skill
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0); // Connection to aquatic creatures increases lore
        SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0); // Allows for greater care for sea creatures

        // Resistances - adding elemental protection
        EnergyBonus = 10; // Minor resistance to energy-based attacks, as the ocean protects
        PhysicalBonus = 5; // Slight resistance to physical attacks, akin to the turtle's shell

        // Special effects tied to the ocean and turtle motifs
        Attributes.ReflectPhysical = 5; // Reflect some physical damage back at attackers
        Attributes.LowerManaCost = 5;  // Decreases mana cost of certain water-based spells

        // XmlLevelItem to add to the XmlSpawner system for dynamic level scaling
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public OceanclaspBands(Serial serial) : base(serial)
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
