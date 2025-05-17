using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TracklessWyrmbinders : HidePants
{
    [Constructable]
    public TracklessWyrmbinders()
    {
        Name = "Trackless Wyrmbinders";
        Hue = Utility.Random(1, 1000); // Color set to a random hue for variety
        BaseArmorRating = Utility.RandomMinMax(10, 30); // Slightly lighter armor for mobility

        // Armor Attributes
        ArmorAttributes.SelfRepair = 5;

        // Attribute bonuses
        Attributes.BonusDex = 15; // Focus on agility and stealth
        Attributes.BonusStam = 10; // Endurance for tracking

        // Skill Bonuses to complement the lore of tracking and wilderness
        SkillBonuses.SetValues(0, SkillName.Tracking, 20.0); // Improve tracking abilities
        SkillBonuses.SetValues(1, SkillName.Veterinary, 15.0); // Expertise with animals and beasts
        SkillBonuses.SetValues(2, SkillName.Stealth, 20.0); // Enhance sneaky movements in the wild

        // Environmental resistances (for wilderness survival)
        ColdBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 15;

        // Attach XmlLevelItem for the item level tracking and spawn functionality
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TracklessWyrmbinders(Serial serial) : base(serial)
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
