using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BakersHexguard : HalfApron
{
    [Constructable]
    public BakersHexguard()
    {
        Name = "Baker's Hexguard";
        Hue = Utility.Random(2300, 2400); // Earthy tones, maybe a rich brown or golden hue

        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 5;
        Attributes.BonusInt = 5;
        Attributes.BonusHits = 10;
        Attributes.BonusStam = 5;

        // Resistances - adding a focus on defense and protection while crafting
        Resistances.Physical = 15;
        Resistances.Fire = 10;  // Protects against heat from the oven and cooking mishaps
        Resistances.Poison = 5; // Some protection from cooking accidents (e.g., spoiled ingredients or alchemy mishaps)

        // Skill Bonuses - Thematically focused on cooking, alchemy, and crafting
        SkillBonuses.SetValues(0, SkillName.Cooking, 10.0);     // Increased cooking skill for better results
        SkillBonuses.SetValues(1, SkillName.Alchemy, 5.0);      // Bonus to alchemy as it ties to culinary experimentation
        SkillBonuses.SetValues(2, SkillName.Healing, 5.0);      // Helps with treating burns or other minor injuries
        SkillBonuses.SetValues(3, SkillName.Tailoring, 5.0);    // Helps with crafting or mending clothes when needed
        SkillBonuses.SetValues(4, SkillName.ItemID, 3.0);       // Improved item identification while crafting or selling goods

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BakersHexguard(Serial serial) : base(serial)
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
