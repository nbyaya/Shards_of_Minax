using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SpinesOfTheQuietRebellion : StuddedGloves
{
    [Constructable]
    public SpinesOfTheQuietRebellion()
    {
        Name = "Spines of the Quiet Rebellion";
        Hue = Utility.Random(1, 1000); // Light, muted colors to represent the hidden nature of the gloves
        BaseArmorRating = Utility.RandomMinMax(10, 30); // Studded gloves typically have lower AR but offer agility

        ArmorAttributes.SelfRepair = 5; // Minor self-repair to signify resilience in rebellion

        Attributes.BonusDex = 15; // Enhances dexterity for stealthy movement and quick strikes
        Attributes.BonusStam = 10; // Provides additional stamina for dodging and prolonged movements
        Attributes.DefendChance = 10; // Helps with evading attacks, fitting for a rebel who avoids direct confrontation
        Attributes.Luck = 25; // The quiet rebel often has a streak of good fortune in their endeavors

        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0); // Rebels must stay hidden, and these gloves enhance stealth skills
        SkillBonuses.SetValues(1, SkillName.Lockpicking, 10.0); // Allows for quiet infiltration and unlocking secrets
        SkillBonuses.SetValues(2, SkillName.Hiding, 10.0); // Bonus to Hiding, since a true rebel knows how to disappear when needed

        ColdBonus = 5; // Minor resistance to cold, signifying the ability to endure harsh conditions while on the run
        PoisonBonus = 5; // Some resistance to poison, possibly from the materials used in the gloves or a metaphor for resisting the toxicity of authority

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attaches the item to the XmlLevelItem for dynamic behavior
    }

    public SpinesOfTheQuietRebellion(Serial serial) : base(serial)
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
