using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SpiralknotObi : Obi
{
    [Constructable]
    public SpiralknotObi()
    {
        Name = "Spiralknot Obi";
        Hue = 1153; // Deep, mystical blue with a hint of shadow.

        // Set attributes and bonuses
        Attributes.BonusInt = 10;
        Attributes.BonusMana = 25;
        Attributes.Luck = 50;
        Attributes.SpellDamage = 8;

        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 15;
        Resistances.Cold = 5;
        Resistances.Poison = 10;
        Resistances.Energy = 15;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Ninjitsu, 10.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);
        SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 10.0);
        SkillBonuses.SetValues(3, SkillName.Meditation, 10.0);
        SkillBonuses.SetValues(4, SkillName.Hiding, 10.0);

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SpiralknotObi(Serial serial) : base(serial)
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
