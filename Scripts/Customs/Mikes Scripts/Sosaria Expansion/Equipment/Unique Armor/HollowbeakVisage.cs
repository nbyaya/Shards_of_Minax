using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HollowbeakVisage : RavenHelm
{
    [Constructable]
    public HollowbeakVisage()
    {
        Name = "Hollowbeak Visage";
        Hue = 1150; // Dark, ominous hue
        BaseArmorRating = Utility.RandomMinMax(30, 50); // Average armor rating for this type of helm

        Attributes.BonusDex = 15; // Increased dexterity, aligning with stealthy or agile movements
        Attributes.BonusInt = 10; // Enhances intellect for magical and perceptive abilities
        Attributes.DefendChance = 5; // Slightly increases the chances to defend against attacks
        Attributes.NightSight = 1; // Provides night vision, befitting the Raven's lore

        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0); // Boosts Stealth skill to aid in remaining undetected
        SkillBonuses.SetValues(1, SkillName.Tracking, 15.0); // Makes it easier to follow others, like a raven tracking prey
        SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 10.0); // Enhances the connection to the spirits and possibly prophetic visions

        ColdBonus = 10; // Slight protection against cold, in line with raven-associated survival
        PhysicalBonus = 5; // Provides a small boost to physical resistance, useful for a stealthy character

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attaches the item to XML for level-based stats or scaling

    }

    public HollowbeakVisage(Serial serial) : base(serial)
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
