using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VelvetfallRadiance : EveningGown
{
    [Constructable]
    public VelvetfallRadiance()
    {
        Name = "Velvetfall Radiance";
        Hue = 1359; // Dark purple, fitting for the elegant theme of the gown
        
        // Set attributes and bonuses
        Attributes.BonusInt = 10;
        Attributes.BonusMana = 15;
        Attributes.Luck = 50;
        Attributes.SpellDamage = 10;
        Attributes.EnhancePotions = 15;
        
        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 10;
        Resistances.Cold = 20;
        Resistances.Poison = 15;
        Resistances.Energy = 5;
        
        // Skill Bonuses - The gown is designed to enhance magical and artistic skills
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0); // For mages or spellcasters
        SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0); // To aid in spellcasting
        SkillBonuses.SetValues(2, SkillName.Inscribe, 15.0); // For those who deal with magical scrolls and writings
        SkillBonuses.SetValues(3, SkillName.Musicianship, 10.0); // Fits with the elegant, artistic theme
        SkillBonuses.SetValues(4, SkillName.Spellweaving, 15.0); // Enhances magical performance

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VelvetfallRadiance(Serial serial) : base(serial)
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
