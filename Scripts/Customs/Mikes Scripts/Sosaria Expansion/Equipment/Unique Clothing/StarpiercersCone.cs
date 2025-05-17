using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarpiercersCone : WizardsHat
{
    [Constructable]
    public StarpiercersCone()
    {
        Name = "Starpiercer's Cone";
        Hue = 1157; // A deep cosmic blue with hints of starlight

        // Set attributes and bonuses
        Attributes.BonusInt = 15;  // Bonus Intelligence to amplify magic power
        Attributes.BonusMana = 30;  // Extra Mana for spellcasting
        Attributes.CastSpeed = 1;  // Faster casting speed
        Attributes.CastRecovery = 1;  // Quick recovery between casts
        Attributes.LowerManaCost = 10;  // Lower the cost of spells

        // Resistances (The cosmos offer a shield against certain elements)
        Resistances.Energy = 15;  // Resists energy-based magic attacks (like from wizards)
        Resistances.Fire = 10;    // Resists fiery elements from the stars

        // Skill Bonuses (Bonus to magery and other related skills)
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);  // Boost Magery for casting power
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);  // Evaluating Intelligence for magic efficiency
        SkillBonuses.SetValues(2, SkillName.Mysticism, 10.0);  // A nod to ancient, star-based magic

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarpiercersCone(Serial serial) : base(serial)
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
