using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GroveboundTunic : Tunic
{
    [Constructable]
    public GroveboundTunic()
    {
        Name = "Grovebound Tunic";
        Hue = Utility.Random(1000, 1200);  // Natural earthy colors

        // Set attributes and bonuses
        Attributes.AttackChance = 10;
        Attributes.LowerManaCost = 10;
        Attributes.LowerRegCost = 12;
        Attributes.EnhancePotions = 20;

        // Resistances
        Resistances.Physical = 15;
        Resistances.Cold = 20;
        Resistances.Poison = 15;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 10.0);    // Connection to nature
        SkillBonuses.SetValues(1, SkillName.Veterinary, 10.0);    // Healing and animal care
        SkillBonuses.SetValues(2, SkillName.Hiding, 10.0);        // Stealth in the forest
        SkillBonuses.SetValues(3, SkillName.Healing, 10.0);       // Herbal knowledge and natural remedies
        SkillBonuses.SetValues(4, SkillName.MagicResist, 10.0);    // Resistance to magical influences in the wild

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GroveboundTunic(Serial serial) : base(serial)
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
