using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperspikeCollar : StuddedGorget
{
    [Constructable]
    public WhisperspikeCollar()
    {
        Name = "Whisperspike Collar";
        Hue = Utility.Random(1200, 1400); // Dark, ethereal tones to match its sinister nature
        BaseArmorRating = 10;

        // Attributes
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 10;
        Attributes.DefendChance = 5;
        Attributes.NightSight = 1; // This ties in with the mysterious and shadowy nature

        // Skill Bonuses to reflect agility and deception
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
        SkillBonuses.SetValues(1, SkillName.Snooping, 15.0);
        SkillBonuses.SetValues(2, SkillName.Lockpicking, 10.0);

        // Resistances
        ColdBonus = 5;
        PoisonBonus = 5;

        // Special Effect - Minor curse or ethereal power
        Attributes.CastSpeed = 1; // Small boost to casting speed, maybe for a stealthy mage or rogue-type character

        // Attach XML Level Item for additional handling
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperspikeCollar(Serial serial) : base(serial)
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
