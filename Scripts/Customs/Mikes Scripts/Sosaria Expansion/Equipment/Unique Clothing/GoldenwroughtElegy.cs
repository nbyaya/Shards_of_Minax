using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GoldenwroughtElegy : GildedDress
{
    [Constructable]
    public GoldenwroughtElegy()
    {
        Name = "Goldenwrought Elegy";
        Hue = 1157;  // Gilded, golden hue

        // Set attributes and bonuses
        Attributes.BonusInt = 15;
        Attributes.BonusHits = 20;
        Attributes.BonusMana = 30;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 5;
        Resistances.Cold = 5;
        Resistances.Poison = 5;
        Resistances.Energy = 15; // Energy resistance ties with its magical nature

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Magery, 10.0);  // Enhances spellcasting abilities
        SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0);  // Increases intelligence for magical power
        SkillBonuses.SetValues(2, SkillName.Mysticism, 10.0);  // Aligns with the mystical arcane theme
        SkillBonuses.SetValues(3, SkillName.Inscribe, 5.0);  // Adds lore and inscribing magical spells

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GoldenwroughtElegy(Serial serial) : base(serial)
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
