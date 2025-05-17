using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChestOfTheRoaringLineage : TigerPeltChest
{
    [Constructable]
    public ChestOfTheRoaringLineage()
    {
        Name = "Chest of the Roaring Lineage";
        Hue = Utility.Random(1100, 1500);  // A deep, earthy tone with stripes like a tiger's pelt
        BaseArmorRating = Utility.RandomMinMax(25, 55);

        // Attributes
        Attributes.BonusStr = 10;
        Attributes.BonusDex = 15;
        Attributes.BonusHits = 30;
        Attributes.DefendChance = 5;
        Attributes.LowerManaCost = 5;
        Attributes.ReflectPhysical = 10;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 25.0);  // Reflecting the tiger and wild animal theme
        SkillBonuses.SetValues(1, SkillName.Veterinary, 20.0);  // Aligning with the lore of the "Roaring Lineage"
        SkillBonuses.SetValues(2, SkillName.Tracking, 15.0);  // Tying into the hunter/nature connection

        // Elemental Resistances (the tiger as a powerful natural creature)
        PhysicalBonus = 15;
        FireBonus = 5;
        PoisonBonus = 5;

        // Unique Features
        ArmorAttributes.SelfRepair = 10;  // The item has an enduring legacy, self-repairing like a predator’s resilience

        // Attach the XmlLevelItem for tracking item data
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChestOfTheRoaringLineage(Serial serial) : base(serial)
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
