using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EyestoneTuner : IntelligenceEvaluator
{
    [Constructable]
    public EyestoneTuner()
    {
        Name = "Eyestone Tuner";
        Hue = Utility.Random(1150, 1200);  // A mystical purple hue that glows with an inner knowledge
        MinDamage = Utility.RandomMinMax(10, 25);  // Light physical damage, meant for spellcasting support
        MaxDamage = Utility.RandomMinMax(30, 55);

        // Attributes focused on increasing magical knowledge and boosting spellcasting efficiency
        Attributes.CastSpeed = 1;
        Attributes.CastRecovery = 2;
        Attributes.BonusInt = 20;  // The item boosts the caster’s intelligence, enhancing their spellcasting
        Attributes.Luck = 15;  // Slight increase in luck for those that depend on their intellect
        Attributes.SpellDamage = 15;  // Enhances overall spell damage output
        
        // Slayer effect – Analyzes and dissects elemental magic in particular, useful for magi dealing with arcane forces
        Slayer = SlayerName.ElementalBan;

        // Skill bonuses for magical disciplines, focusing on the deep understanding of magic and insight into the self
        SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);  // Evaluates intelligence and enhances magical understanding
        SkillBonuses.SetValues(1, SkillName.Magery, 15.0);   // Enhances spellcasting abilities
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);  // Helps the caster to meditate and focus more effectively
        
        // Additional bonus for deeper arcane research and understanding
        SkillBonuses.SetValues(3, SkillName.Spellweaving, 10.0);  // Empowering the caster with greater arcane power and insight into complex spells

        // Custom property indicating this is a unique item
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EyestoneTuner(Serial serial) : base(serial)
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
