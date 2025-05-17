using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WayfarersLuckhorn : TricorneHat
{
    [Constructable]
    public WayfarersLuckhorn()
    {
        Name = "Wayfarer's Luckhorn";
        Hue = Utility.Random(1000, 1500); // Give it a weathered look, perhaps brown with gold accents
        
        // Set attributes and bonuses

        Attributes.BonusStam = 10;
        Attributes.BonusMana = 5;
        Attributes.Luck = 75;
        Attributes.DefendChance = 5;
        Attributes.NightSight = 1;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Tracking, 15.0);  // Ideal for a wayfarer
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);  // Custom skill for the wayfarer theme (could be an exploration-based skill)
        SkillBonuses.SetValues(2, SkillName.Camping, 10.0);  // Camping and exploration themed bonus
        SkillBonuses.SetValues(3, SkillName.Healing, 5.0);  // Self-sufficiency in wilderness settings

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WayfarersLuckhorn(Serial serial) : base(serial)
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
