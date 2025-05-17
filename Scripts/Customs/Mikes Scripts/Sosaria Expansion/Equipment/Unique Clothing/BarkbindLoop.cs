using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BarkbindLoop : WoodlandBelt
{
    [Constructable]
    public BarkbindLoop()
    {
        Name = "Barkbind Loop";
        Hue = 1360; // Forest green color with hints of brown

        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Physical = 12;
        Resistances.Cold = 8;
        Resistances.Poison = 15;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 15.0); // Enhances lumberjacking, fitting with nature theme
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0); // Knowledge of animals, an earthy connection
        SkillBonuses.SetValues(2, SkillName.Tracking, 10.0); // Increases tracking, for those who follow trails
        SkillBonuses.SetValues(3, SkillName.Healing, 5.0); // Encourages a naturalistic healing approach with plants and herbs

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BarkbindLoop(Serial serial) : base(serial)
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
