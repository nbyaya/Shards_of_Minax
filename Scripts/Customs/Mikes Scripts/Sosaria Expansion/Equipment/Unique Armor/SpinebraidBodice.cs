using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SpinebraidBodice : FemaleStuddedChest
{
    [Constructable]
    public SpinebraidBodice()
    {
        Name = "Spinebraid Bodice";
        Hue = Utility.Random(1, 1000); // Random color for uniqueness
        BaseArmorRating = Utility.RandomMinMax(30, 70); // Good armor rating for chest armor

        // Attributes
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 10;
        Attributes.DefendChance = 10; // Increased defense chance
        Attributes.ReflectPhysical = 10; // Reflects physical damage

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Tailoring, 15.0); // Tailoring for crafting knowledge
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0); // Animal Lore for nature connection
        SkillBonuses.SetValues(2, SkillName.Stealth, 10.0); // Bonus to Stealth to aid in agility

        // Elemental Bonuses
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 5;
        PoisonBonus = 5;

        // Attach the XML Level Item
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SpinebraidBodice(Serial serial) : base(serial)
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
