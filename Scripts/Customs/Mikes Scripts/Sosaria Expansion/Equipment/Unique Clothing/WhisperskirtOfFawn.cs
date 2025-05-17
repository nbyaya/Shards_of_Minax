using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperskirtOfFawn : Skirt
{
    [Constructable]
    public WhisperskirtOfFawn()
    {
        Name = "Whisperskirt of Fawn";
        Hue = 1150; // Earthy, muted tones to reflect the forest theme

        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 5;
        Attributes.BonusHits = 15;
        Attributes.BonusStam = 10;
        Attributes.BonusMana = 5;
        Attributes.RegenHits = 2;
        Attributes.RegenStam = 3;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Physical = 5;
        Resistances.Cold = 5;
        Resistances.Poison = 10;

        // Skill Bonuses - These skills complement the nature and stealth themes
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0); // Knowledge of creatures and nature
        SkillBonuses.SetValues(1, SkillName.Healing, 10.0); // Tied to the restorative nature of the fawn
        SkillBonuses.SetValues(2, SkillName.Stealth, 20.0); // Stealthy movement, in tune with nature
        SkillBonuses.SetValues(3, SkillName.Veterinary, 15.0); // Care for animals, further enhancing connection with nature
        SkillBonuses.SetValues(4, SkillName.Tracking, 10.0); // Ability to track creatures through the wild

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperskirtOfFawn(Serial serial) : base(serial)
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
