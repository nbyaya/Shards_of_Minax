using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class UmbralEmbrace : HoodedShroudOfShadows
{
    [Constructable]
    public UmbralEmbrace()
    {
        Name = "Umbral Embrace";
        Hue = 1153; // Dark, shadowy color for the cloak.
        
        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 10;
        Attributes.BonusHits = 10;
        Attributes.BonusStam = 10;
        Attributes.BonusMana = 15;

        // Resistances (to represent the shroud's protection against physical and magical elements)
        Resistances.Physical = 15;
        Resistances.Fire = 5;
        Resistances.Cold = 10;
        Resistances.Poison = 20;
        Resistances.Energy = 10;

        // Skill Bonuses (shadowy, stealth, and necromantic theme)
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        SkillBonuses.SetValues(1, SkillName.Necromancy, 20.0);
        SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 15.0);
        SkillBonuses.SetValues(3, SkillName.Hiding, 15.0);
        SkillBonuses.SetValues(4, SkillName.Meditation, 10.0); // Helps with mana regeneration and maintaining focus in shadows

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public UmbralEmbrace(Serial serial) : base(serial)
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
