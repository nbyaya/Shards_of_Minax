using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WyrmhideBreeches : LongPants
{
    [Constructable]
    public WyrmhideBreeches()
    {
        Name = "Wyrmhide Breeches";
        Hue = Utility.Random(2300, 2500);  // Dark dragon-like hues to signify wyrm power
        
        // Set attributes and bonuses
        Attributes.BonusStr = 15;
        Attributes.BonusDex = 10;
        Attributes.BonusHits = 25;
        Attributes.BonusStam = 20;
        Attributes.DefendChance = 10;
        Attributes.ReflectPhysical = 15;
        Attributes.Luck = 50;
        Attributes.SpellDamage = 5;
        
        // Resistances - Dragonkin and wyrm-like toughness
        Resistances.Physical = 20;
        Resistances.Fire = 15;
        Resistances.Cold = 10;
        Resistances.Poison = 15;
        
        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);   // Perfect for a warrior that survives dragon battles
        SkillBonuses.SetValues(1, SkillName.Mining, 10.0);     // Because of the lore of wyrm-related mineral power, this is fitting
        SkillBonuses.SetValues(2, SkillName.MagicResist, 5.0); // Provides protection against magic
        SkillBonuses.SetValues(3, SkillName.Anatomy, 10.0);    // A nod to the deep understanding of bodily endurance
    
        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WyrmhideBreeches(Serial serial) : base(serial)
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
