using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BannercladSkirtOfTheFirstHold : GuildedKilt
{
    [Constructable]
    public BannercladSkirtOfTheFirstHold()
    {
        Name = "Bannerclad Skirt of the First Hold";
        Hue = Utility.Random(1000, 2000); // Gold and red hues for a noble appearance.
        
        // Set attributes and bonuses
        Attributes.BonusStr = 10;
        Attributes.BonusDex = 10;
        Attributes.BonusHits = 15;
        Attributes.RegenHits = 3;
        Attributes.DefendChance = 5;
        Attributes.LowerManaCost = 5;
        Attributes.Luck = 50;
        
        // Resistances
        Resistances.Physical = 15;
        Resistances.Fire = 10;
        Resistances.Cold = 5;

        // Skill Bonuses - Focused on defense, combat, and leadership
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // Leadership and strategy in combat.
        SkillBonuses.SetValues(1, SkillName.ArmsLore, 10.0); // Understanding of armor and combat proficiency.
        SkillBonuses.SetValues(2, SkillName.Parry, 10.0); // Enhances defense and ability to deflect attacks.
        SkillBonuses.SetValues(3, SkillName.Swords, 5.0); // Improves sword-fighting capability in battle.

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BannercladSkirtOfTheFirstHold(Serial serial) : base(serial)
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
