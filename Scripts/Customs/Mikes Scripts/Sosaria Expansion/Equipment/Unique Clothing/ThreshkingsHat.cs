using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThreshkingsHat : TallStrawHat
{
    [Constructable]
    public ThreshkingsHat()
    {
        Name = "Threshking's Hat";
        Hue = Utility.Random(2500, 2550);  // A golden hue, representing the harvest season
        
        // Set attributes and bonuses

        Attributes.BonusHits = 10;
        Attributes.BonusStam = 10;
        Attributes.BonusMana = 10;
        Attributes.Luck = 30;

        // Resistances (symbolic of nature's resilience)
        Resistances.Physical = 5;
        Resistances.Cold = 5;
        Resistances.Poison = 5;

        // Skill Bonuses (focused on nature and farming, fitting for a harvest-related item)
        SkillBonuses.SetValues(0, SkillName.Alchemy, 10.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);
        SkillBonuses.SetValues(2, SkillName.Cooking, 10.0);
        SkillBonuses.SetValues(3, SkillName.Herding, 5.0);

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThreshkingsHat(Serial serial) : base(serial)
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
