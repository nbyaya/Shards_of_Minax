using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class UndertideTreads : DragonTurtleHideLegs
{
    [Constructable]
    public UndertideTreads()
    {
        Name = "Undertide Treads";
        Hue = Utility.Random(1150, 1400); // Earth and water-themed hues (blue and earthy green)
        BaseArmorRating = Utility.RandomMinMax(15, 45); // Balanced armor rating for legs

        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;

        // Elemental Resistance
        ColdBonus = 10;
        PhysicalBonus = 10;

        // Thematically appropriate skills: 
        SkillBonuses.SetValues(0, SkillName.Fishing, 10.0); // Tie to aquatic survival
        SkillBonuses.SetValues(1, SkillName.Veterinary, 15.0); // Connection to nature and beasts
        SkillBonuses.SetValues(2, SkillName.Tracking, 10.0); // Survivalist skill, especially on terrain

        // Armor-related attributes: for resilience and survival
        ArmorAttributes.SelfRepair = 5; // Moderate self-repair to reflect the durability of dragon turtle hide

        // Unique enchantments that boost survivability
        Attributes.DefendChance = 5; // Small defensive boost
        Attributes.RegenStam = 3; // Moderate stamina regeneration to reflect the turtle's enduring nature

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach to the XML system for level-based items
    }

    public UndertideTreads(Serial serial) : base(serial)
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
