using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LaughingCrownOfEchoes : JesterHat
{
    [Constructable]
    public LaughingCrownOfEchoes()
    {
        Name = "Laughing Crown of Echoes";
        Hue = 1157; // A vibrant color that matches the joyful yet eerie tone of the item
        
        // Set attributes and bonuses
        Attributes.BonusDex = 15;
        Attributes.Luck = 50; // Jester-themed bonus for luck
        Attributes.CastSpeed = 1; // Jester's quick wit
        Attributes.EnhancePotions = 10; // For the potion-related antics a jester might engage in

        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 5;
        Resistances.Poison = 5;
        Resistances.Energy = 5;

        // Skill Bonuses - All themed around charm, deception, and agility
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 10.0); // A jester’s ability to charm others
        SkillBonuses.SetValues(1, SkillName.Provocation, 15.0); // Mastery in provoking laughs or chaos
        SkillBonuses.SetValues(2, SkillName.Stealth, 10.0); // Stealthy movements like a sneaky performer
        SkillBonuses.SetValues(3, SkillName.Discordance, 10.0); // Manipulating the environment with mischief

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LaughingCrownOfEchoes(Serial serial) : base(serial)
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
