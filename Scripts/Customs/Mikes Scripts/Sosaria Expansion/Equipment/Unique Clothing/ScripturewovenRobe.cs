using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ScripturewovenRobe : Robe
{
    [Constructable]
    public ScripturewovenRobe()
    {
        Name = "Scripturewoven Robe";
        Hue = Utility.Random(1000, 2000); // A mystical hue to reflect its arcane and sacred origins
        
        // Set attributes and bonuses
        Attributes.BonusInt = 15; // For the wisdom and understanding required by scholars and mages
        Attributes.BonusMana = 25; // Provides additional mana to assist in spellcasting
        Attributes.RegenMana = 5; // Regeneration of mana for prolonged magical use
        Attributes.CastSpeed = 1; // Faster casting speed for spellcasters
        Attributes.LowerManaCost = 10; // Reduces the cost of casting spells

        // Resistances
        Resistances.Fire = 15; // The robe is known to resist fire due to its magical properties
        Resistances.Cold = 15; // Protects the wearer against the cold touch of the void
        Resistances.Poison = 10; // A moderate resistance to poison, reflecting its protection against corruption
        Resistances.Energy = 20; // A stronger energy resistance, as the robe channels raw mystical power
        
        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0); // Perfect for those who scribe magical texts or arcane formulas
        SkillBonuses.SetValues(1, SkillName.Magery, 15.0); // Ideal for mages who practice the art of spellcasting
        SkillBonuses.SetValues(2, SkillName.EvalInt, 10.0); // Enhances the mage's understanding of magical forces and the arcane
        SkillBonuses.SetValues(3, SkillName.Mysticism, 10.0); // Those attuned to the ancient forces of mysticism will find this robe highly beneficial
        
        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ScripturewovenRobe(Serial serial) : base(serial)
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
