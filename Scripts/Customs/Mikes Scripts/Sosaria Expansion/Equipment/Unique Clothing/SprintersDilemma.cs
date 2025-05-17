using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SprintersDilemma : ShortPants
{
    [Constructable]
    public SprintersDilemma()
    {
        Name = "Sprinter’s Dilemma";
        Hue = Utility.Random(2300, 2400); // A color that feels swift, like a vibrant, energetic hue
        
        // Set attributes and bonuses
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.RegenStam = 5;
        Attributes.LowerManaCost = 5;
        Attributes.Luck = 50;
        Attributes.WeaponSpeed = 20;
        Attributes.DefendChance = 5;
        Attributes.AttackChance = 10;

        // Resistances
        Resistances.Physical = 10;
        Resistances.Fire = 5;
        Resistances.Cold = 5;
        Resistances.Poison = 5;
        Resistances.Energy = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);  // Fits with speed and sneaky actions
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0); // Tracking enhances the mobility aspect
        SkillBonuses.SetValues(2, SkillName.Hiding, 10.0);   // Useful for quick concealment
        SkillBonuses.SetValues(3, SkillName.Meditation, 5.0); // Useful for maintaining stamina and focus during fast movements
        SkillBonuses.SetValues(4, SkillName.Tactics, 5.0);    // Enhances combat awareness during quick movement
        
        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SprintersDilemma(Serial serial) : base(serial)
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
