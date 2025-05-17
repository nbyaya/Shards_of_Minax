using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ReapersWeft : DeathRobe
{
    [Constructable]
    public ReapersWeft()
    {
        Name = "Reaper's Weft";
        Hue = 1157; // A dark, shadowy color fitting for the theme of the Reaper

        // Set attributes and bonuses

        Attributes.BonusInt = 15;
        Attributes.BonusHits = 20;
        Attributes.BonusMana = 30;
        Attributes.LowerManaCost = 10;
        Attributes.LowerRegCost = 10;

        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 5;
        Resistances.Cold = 10;
        Resistances.Poison = 15;
        Resistances.Energy = 20;

        // Skill Bonuses (Thematically fitting Necromancy, Stealth, and Spellcasting)
        SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0); // Enhances Necromancy for the Reaper theme
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0); // Improves SpiritSpeak, essential for reapers and necromancers
        SkillBonuses.SetValues(2, SkillName.Stealth, 10.0); // Stealth helps the reaper move in the shadows
        SkillBonuses.SetValues(3, SkillName.Magery, 5.0); // Magery bonus for spellcasting support
        SkillBonuses.SetValues(4, SkillName.EvalInt, 5.0); // Increases spell effectiveness with EvalInt for necromantic spells

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ReapersWeft(Serial serial) : base(serial)
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
