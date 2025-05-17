using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TreadOfTheThornMarch : StuddedLegs
{
    [Constructable]
    public TreadOfTheThornMarch()
    {
        Name = "Tread of the Thorn March";
        Hue = Utility.Random(1000, 1500); // Dark greenish hues with a touch of brown, like forest earth
        BaseArmorRating = Utility.RandomMinMax(15, 40); // Lower base armor rating to reflect its focus on agility

        // Attributes that fit the armor's nature theme
        Attributes.BonusDex = 15; // Boosts dexterity for increased movement and agility
        Attributes.BonusStam = 10; // Enhances stamina for better endurance in the wild
        Attributes.DefendChance = 10; // Gives a bonus to defense while moving through harsh environments

        // Skill Bonuses that enhance the survivalist nature and stealth
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0); // Stealth to move unseen through the wilderness
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0); // Tracking for identifying creatures and survival in the wild
        SkillBonuses.SetValues(2, SkillName.Anatomy, 5.0); // Basic anatomy to make better use of terrain and enemies' weaknesses

        // Elemental bonuses, subtle connection to nature's resilience
        PhysicalBonus = 15; // Offers additional physical defense, representing hardened natural materials
        PoisonBonus = 10; // Defense against poison, useful for navigating toxic wilds

        // Special Effects - Tread lightly and avoid detection
        Attributes.Luck = 15; // Bonus to luck for those who live on the fringes, benefiting from the elements
        Attributes.RegenStam = 5; // Regeneration of stamina to keep up with the tough terrain

        // Attach XmlLevelItem to this custom armor item for level scaling
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TreadOfTheThornMarch(Serial serial) : base(serial)
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
