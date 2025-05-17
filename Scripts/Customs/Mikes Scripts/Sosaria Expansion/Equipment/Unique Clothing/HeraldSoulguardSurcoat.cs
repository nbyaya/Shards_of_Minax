using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HeraldSoulguardSurcoat : Surcoat
{
    [Constructable]
    public HeraldSoulguardSurcoat()
    {
        Name = "Herald's Soulguard";
        Hue = 1150; // A noble, regal color

        // Set attributes and bonuses
        Attributes.BonusHits = 30;
        Attributes.RegenHits = 5;
        Attributes.LowerManaCost = 10;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Physical = 15;
        Resistances.Fire = 10;
        Resistances.Cold = 10;
        Resistances.Poison = 5;
        Resistances.Energy = 20;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Chivalry, 15.0); // Enhancing their holy cause
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0); // For better combat awareness
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0); // To tend to wounds in battle
        SkillBonuses.SetValues(3, SkillName.Healing, 10.0); // Helping with self and allies' recovery
        SkillBonuses.SetValues(4, SkillName.Necromancy, 5.0); // Subtle connection to dark history

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HeraldSoulguardSurcoat(Serial serial) : base(serial)
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
