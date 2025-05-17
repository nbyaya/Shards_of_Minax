using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SilentVowOfTheWatcher : CloseHelm
{
    [Constructable]
    public SilentVowOfTheWatcher()
    {
        Name = "Silent Vow of the Watcher";
        Hue = Utility.Random(1150, 1300); // Mysterious dark hues to match the stealthy nature
        BaseArmorRating = Utility.RandomMinMax(25, 50); // Balanced armor for stealth and defense

        // Attributes related to stealth, perception, and defense
        Attributes.DefendChance = 10; // Increases the chances of dodging attacks
        Attributes.NightSight = 1; // Provides sight in darkness, symbolic of the Watcher's ability to see in the dark
        Attributes.LowerManaCost = 5; // Lowers the mana cost for using stealth-related magic like invisibility or shadow-based spells

        // Skill bonuses that fit the thematic focus on stealth, observation, and combat awareness
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0); // Enhances stealth capabilities
        SkillBonuses.SetValues(1, SkillName.DetectHidden, 15.0); // Increases perception, allowing the wearer to detect hidden threats
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0); // Boosts combat tactics, as the wearer knows when to strike in a stealthy manner

        // Elemental bonuses for thematic enhancement of shadowy and elusive nature
        ColdBonus = 5;
        EnergyBonus = 10;
        PhysicalBonus = 10;

        // XmlLevelItem attachment to track level-related data for future scaling
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SilentVowOfTheWatcher(Serial serial) : base(serial)
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
