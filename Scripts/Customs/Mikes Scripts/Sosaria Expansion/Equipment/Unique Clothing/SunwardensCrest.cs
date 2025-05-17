using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SunwardensCrest : WideBrimHat
{
    [Constructable]
    public SunwardensCrest()
    {
        Name = "Sunwarden's Crest";
        Hue = 1154;  // Golden color with sun-like hue
        
        // Set attributes and bonuses
        Attributes.RegenStam = 5;
        Attributes.LowerManaCost = 10;
        Attributes.ReflectPhysical = 5;
        Attributes.Luck = 50;
        
        // Resistances (fitting for a sun-themed protector)
        Resistances.Physical = 10;
        Resistances.Fire = 20;
        Resistances.Cold = 5;
        Resistances.Poison = 10;
        Resistances.Energy = 10;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Healing, 10.0); // A protector's touch
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0); // Knowledge of nature
        SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0); // Bond with creatures
        SkillBonuses.SetValues(3, SkillName.AnimalLore, 10.0); // Understanding of animals
        SkillBonuses.SetValues(4, SkillName.Parry, 10.0); // Defense against attackers
        
        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SunwardensCrest(Serial serial) : base(serial)
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
