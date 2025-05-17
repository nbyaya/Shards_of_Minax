using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StonefireTartanKilt : Kilt
{
    [Constructable]
    public StonefireTartanKilt()
    {
        Name = "Stonefire Tartan Kilt";
        Hue = 1200; // A fiery, earthy red tone that matches the Stonefire theme
        
        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 8;
        Attributes.BonusInt = 3;
        Attributes.BonusHits = 15;
        Attributes.BonusStam = 15;
        Attributes.BonusMana = 10;
        Attributes.RegenHits = 2;
        Attributes.RegenStam = 2;
        Attributes.RegenMana = 2;
        Attributes.DefendChance = 10;
        Attributes.Luck = 50;
        
        // Resistances
        Resistances.Physical = 12;
        Resistances.Fire = 20;
        Resistances.Cold = 5;
        Resistances.Poison = 5;
        Resistances.Energy = 10;

        // Skill Bonuses (fire and nature-themed skills)
        SkillBonuses.SetValues(0, SkillName.Archery, 5.0);  // Fire and precision
        SkillBonuses.SetValues(1, SkillName.Tactics, 7.5);   // Combat readiness in rough terrain
        SkillBonuses.SetValues(2, SkillName.Mining, 5.0);    // Tied to the mountain stone theme
        SkillBonuses.SetValues(3, SkillName.Anatomy, 5.0);   // Knowing the body, like the earth's anatomy
        SkillBonuses.SetValues(4, SkillName.Tracking, 5.0);  // Tracking across rugged, fiery landscapes
        
        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StonefireTartanKilt(Serial serial) : base(serial)
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
