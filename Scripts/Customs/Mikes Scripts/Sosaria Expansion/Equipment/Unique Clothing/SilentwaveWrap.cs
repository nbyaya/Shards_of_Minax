using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SilentwaveWrap : Bandana
{
    [Constructable]
    public SilentwaveWrap()
    {
        Name = "Silentwave Wrap";
        Hue = 1100; // Subtle gray-blue hue to reflect the silent, wave-like quality.

        // Set attributes and bonuses
        Attributes.BonusDex = 15; // Enhances dexterity for improved stealth and agility.
        Attributes.BonusStam = 10; // Increased stamina for endurance.
        Attributes.RegenStam = 2; // Slow stamina regeneration to aid in prolonged stealth movements.
        Attributes.Luck = 50; // Adds some luck to aid in avoiding detection.
        Attributes.NightSight = 1; // Provides night vision for stealth operations in the dark.

        // Resistances
        Resistances.Physical = 5; // Minimal physical resistance, as it's a light wrap.
        Resistances.Fire = 2; // Very light fire resistance, adding to its subtle nature.
        Resistances.Cold = 3; // Cold resistance to allow for stealth even in harsh environments.

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0); // Major boost to stealth skill, key for evading detection.
        SkillBonuses.SetValues(1, SkillName.Hiding, 15.0); // Further enhances hiding, essential for blending into the environment.
        SkillBonuses.SetValues(2, SkillName.Ninjitsu, 10.0); // Boost to Ninjitsu, for those who specialize in stealth combat or evasive techniques.

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SilentwaveWrap(Serial serial) : base(serial)
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
