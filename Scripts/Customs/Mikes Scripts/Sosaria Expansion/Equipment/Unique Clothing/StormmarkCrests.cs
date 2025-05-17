using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StormmarkCrests : Epaulette
{
    [Constructable]
    public StormmarkCrests()
    {
        Name = "Stormmark Crests";
        Hue = 1152; // A stormy grey-blue hue
        
        // Set attributes and bonuses
        Attributes.BonusHits = 15;
        Attributes.DefendChance = 10;
        Attributes.Luck = 25;
        Attributes.SpellDamage = 5;
        
        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 10;
        Resistances.Cold = 15;
        Resistances.Poison = 5;
        Resistances.Energy = 20;

        // Skill Bonuses (thematically storm-related)
        SkillBonuses.SetValues(0, SkillName.MagicResist, 20.0); // Resist the force of storms
        SkillBonuses.SetValues(1, SkillName.Focus, 10.0); // Focus and stability in the face of chaos
        SkillBonuses.SetValues(2, SkillName.Tactics, 15.0); // Strategic use of weather’s fury in battle
        SkillBonuses.SetValues(3, SkillName.Healing, 10.0); // The restorative power of the storm’s cleansing rains

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StormmarkCrests(Serial serial) : base(serial)
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
