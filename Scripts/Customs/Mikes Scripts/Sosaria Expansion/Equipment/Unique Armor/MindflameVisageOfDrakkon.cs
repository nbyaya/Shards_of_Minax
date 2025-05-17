using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MindflameVisageOfDrakkon : DragonHelm
{
    [Constructable]
    public MindflameVisageOfDrakkon()
    {
        Name = "Mindflame Visage of Drakkon";
        Hue = Utility.Random(1001, 1150); // Flame-inspired hue
        BaseArmorRating = Utility.RandomMinMax(40, 80); // Strong, Dragon-based defense

        // Adding Attributes and Bonuses
        Attributes.BonusStr = 15; // Bonus strength, as Drakkon is known for his raw power
        Attributes.BonusInt = 25; // Increased Intelligence, enhancing Mysticism and the power of Drakkon's mind
        Attributes.BonusHits = 50; // More hit points, reflecting the resilience of a Dragon

        // Skill Bonuses (thematically tied to fire and mysticism)
        SkillBonuses.SetValues(0, SkillName.Mysticism, 20.0); // Drakkon’s legacy would boost magical abilities
        SkillBonuses.SetValues(1, SkillName.Tactics, 20.0); // Mastery in tactics, as Drakkon was a seasoned warrior
        SkillBonuses.SetValues(2, SkillName.MagicResist, 15.0); // Magic resist, as a Dragon King would be resistant to arcane forces

        // Elemental Bonuses to reflect Drakkon’s affinity with fire
        FireBonus = 20; // Enhanced fire resistance, as Drakkon was a fire-breathing Dragon
        PhysicalBonus = 10; // A general boost in physical resistance

        // Other Special Attributes for unique gameplay
        Attributes.RegenHits = 2; // Minor regeneration to reflect Drakkon’s inherent vitality
        Attributes.DefendChance = 10; // Increased defense chance, symbolizing the armor’s strength

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach the XmlLevelItem for unique properties
    }

    public MindflameVisageOfDrakkon(Serial serial) : base(serial)
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
