using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LoomsoledWanderers : Shoes
{
    [Constructable]
    public LoomsoledWanderers()
    {
        Name = "Loomsoled Wanderers";
        Hue = 1157; // A light blue hue to evoke the feeling of airy, natural footwear
        
        // Set attributes and bonuses
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.RegenStam = 2;
        Attributes.Luck = 50;
        
        // Resistances
        Resistances.Physical = 5;
        Resistances.Cold = 5;
        
        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Tracking, 10.0); // The shoes enhance your ability to track
        SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);  // They aid in stealth for quiet wandering
        SkillBonuses.SetValues(2, SkillName.Camping, 10.0);  // The shoes are perfect for setting up camp in the wild
        
        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LoomsoledWanderers(Serial serial) : base(serial)
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
