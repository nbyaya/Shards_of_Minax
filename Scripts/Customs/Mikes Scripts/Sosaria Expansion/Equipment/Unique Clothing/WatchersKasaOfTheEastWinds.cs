using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WatchersKasaOfTheEastWinds : Kasa
{
    [Constructable]
    public WatchersKasaOfTheEastWinds()
    {
        Name = "Watcher’s Kasa of the East Winds";
        Hue = 1153; // Light, airy hue that fits the theme of the East Winds
        
        // Set attributes and bonuses
        Attributes.BonusDex = 5;
        Attributes.BonusInt = 10;
        Attributes.BonusStam = 5;
        Attributes.RegenMana = 3;

        
        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 5;
        Resistances.Cold = 15;
        Resistances.Poison = 5;
        Resistances.Energy = 10;
        
        // Skill Bonuses (Thematically appropriate)
        SkillBonuses.SetValues(0, SkillName.Tracking, 10.0); // Tied to wind movement and nature's call
        SkillBonuses.SetValues(1, SkillName.Veterinary, 5.0); // Tied to nature and animal care
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0); // Reflects the peaceful nature of the winds
        SkillBonuses.SetValues(3, SkillName.AnimalLore, 5.0); // Tied to understanding nature and its creatures

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WatchersKasaOfTheEastWinds(Serial serial) : base(serial)
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
