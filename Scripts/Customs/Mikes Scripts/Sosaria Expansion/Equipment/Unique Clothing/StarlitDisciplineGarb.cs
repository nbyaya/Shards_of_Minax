using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarlitDisciplineGarb : Kamishimo
{
    [Constructable]
    public StarlitDisciplineGarb()
    {
        Name = "Starlit Discipline Garb";
        Hue = 1157; // A celestial hue, like starlight or moonlight
        
        // Set attributes and bonuses
        Attributes.BonusInt = 15;
        Attributes.BonusMana = 25;
        Attributes.LowerManaCost = 10;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Fire = 5;
        Resistances.Cold = 5;
        Resistances.Energy = 10;

        // Skill Bonuses - Thematically related to discipline and spiritual focus
        SkillBonuses.SetValues(0, SkillName.Meditation, 20.0); // Strengthening focus
        SkillBonuses.SetValues(1, SkillName.Spellweaving, 15.0); // Tapping into celestial magic
        SkillBonuses.SetValues(2, SkillName.Ninjitsu, 10.0); // Stealth and discipline
        SkillBonuses.SetValues(3, SkillName.Swords, 10.0); // Physical discipline through swordsmanship

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarlitDisciplineGarb(Serial serial) : base(serial)
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
