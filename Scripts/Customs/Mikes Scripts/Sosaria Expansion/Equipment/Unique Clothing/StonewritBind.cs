using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StonewritBind : GargishSash
{
    [Constructable]
    public StonewritBind()
    {
        Name = "Stonewrit Bind";
        Hue = 1157; // A stone-like hue, representing ancient stone carvings.
        
        // Set attributes and bonuses
        Attributes.BonusMana = 15;
        Attributes.RegenMana = 3;
        Attributes.LowerManaCost = 10;
        Attributes.DefendChance = 5;
        
        // Resistances
        Resistances.Physical = 12;
        Resistances.Energy = 8;
        
        // Skill Bonuses (Thematically related to stone and ancient knowledge)
        SkillBonuses.SetValues(0, SkillName.Mysticism, 10.0); // Ties to ancient, arcane lore
        SkillBonuses.SetValues(1, SkillName.Magery, 10.0); // A mastery of ancient magic
        SkillBonuses.SetValues(2, SkillName.Alchemy, 5.0); // Stone and alchemy often go hand in hand
        SkillBonuses.SetValues(3, SkillName.Blacksmith, 5.0); // Ties to the craftsmanship of stonecarving
        SkillBonuses.SetValues(4, SkillName.Carpentry, 5.0); // Expertise in handling natural, earthbound materials
        
        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StonewritBind(Serial serial) : base(serial)
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
