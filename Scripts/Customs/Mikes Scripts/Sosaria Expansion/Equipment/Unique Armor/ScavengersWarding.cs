using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ScavengersWarding : VultureHelm
{
    [Constructable]
    public ScavengersWarding()
    {
        Name = "Scavenger's Warding";
        Hue = 1161;  // Dark grayish hue, representing the scavenger aspect.
        BaseArmorRating = Utility.RandomMinMax(25, 50); // A balanced armor rating for a mid-tier item.

        Attributes.BonusStr = 5; // A little strength boost, as scavengers need to carry their finds.
        Attributes.BonusDex = 10; // Increased dexterity for agility in searching and looting.
        Attributes.DefendChance = 10; // Scavengers are elusive, making them harder to hit.
        Attributes.Luck = 15; // Enhancing the chance for lucky finds or more valuable items from kills.

        SkillBonuses.SetValues(0, SkillName.Anatomy, 15.0); // Scavengers need knowledge of anatomy to loot efficiently.
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0); // Tracking ability to find hidden loot or enemies.
        SkillBonuses.SetValues(2, SkillName.Snooping, 10.0); // A boost to snooping for scavenging useful items.

        ColdBonus = 5; // Some protection from the cold, since scavengers often wander in harsh environments.
        PhysicalBonus = 5; // Light physical defense to keep scavengers protected in combat.

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attaches level-specific functionality to the item.
    }

    public ScavengersWarding(Serial serial) : base(serial)
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
