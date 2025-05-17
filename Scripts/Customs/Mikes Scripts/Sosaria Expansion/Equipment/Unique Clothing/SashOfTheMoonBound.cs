using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SashOfTheMoonBound : BodySash
{
    [Constructable]
    public SashOfTheMoonBound()
    {
        Name = "Sash of the Moon-Bound";
        Hue = 1153;  // Moonlit hue, soft glow effect

        // Set attributes and bonuses
        Attributes.BonusMana = 30; 
        Attributes.BonusInt = 15;
        Attributes.BonusHits = 15;


        // Resistances - celestial and arcane-themed
        Resistances.Physical = 5;
        Resistances.Fire = 5;
        Resistances.Cold = 5;
        Resistances.Poison = 10;
        Resistances.Energy = 15;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0); // Enhances magical prowess
        SkillBonuses.SetValues(1, SkillName.Mysticism, 15.0); // Provides a deeper connection with the arcane arts
        SkillBonuses.SetValues(2, SkillName.Spellweaving, 10.0); // Taps into weaving moonlit magic
        SkillBonuses.SetValues(3, SkillName.Alchemy, 10.0); // Alchemical knowledge, perfect for crafting moon-based potions

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SashOfTheMoonBound(Serial serial) : base(serial)
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
