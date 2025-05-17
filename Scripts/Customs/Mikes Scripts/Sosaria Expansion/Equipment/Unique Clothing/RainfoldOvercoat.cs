using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RainfoldOvercoat : JinBaori
{
    [Constructable]
    public RainfoldOvercoat()
    {
        Name = "Rainfold Overcoat";
        Hue = 1157; // Soft grayish color, reminiscent of rainclouds

        // Set attributes and bonuses
        Attributes.BonusInt = 15;
        Attributes.BonusHits = 25;
        Attributes.BonusStam = 15;
        Attributes.BonusMana = 10;


        // Resistances (Protection against rain, weather, and magical energies)
        Resistances.Physical = 10;
        Resistances.Fire = 5;
        Resistances.Cold = 15;
        Resistances.Poison = 10;
        Resistances.Energy = 5;

        // Skill Bonuses (The skills should reflect the overcoat’s connection to nature, resilience, and adaptability)
        SkillBonuses.SetValues(0, SkillName.Camping, 15.0); // For those who wander in nature
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0); // Understanding of animals affected by the weather
        SkillBonuses.SetValues(2, SkillName.Healing, 10.0); // Rainy conditions need healing skills for travelers
        SkillBonuses.SetValues(3, SkillName.Mysticism, 15.0); // Elemental magic connection, control over the rain's forces

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RainfoldOvercoat(Serial serial) : base(serial)
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
