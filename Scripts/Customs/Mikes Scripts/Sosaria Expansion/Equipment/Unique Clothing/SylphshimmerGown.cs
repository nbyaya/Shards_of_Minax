using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SylphshimmerGown : FemaleElvenRobe
{
    [Constructable]
    public SylphshimmerGown()
    {
        Name = "Sylphshimmer Gown";
        Hue = 1157;  // A soft, ethereal greenish-blue, reminiscent of Elven magic
        
        // Set attributes and bonuses
        Attributes.BonusInt = 15;
        Attributes.BonusMana = 25;
        Attributes.RegenMana = 5;
        Attributes.LowerManaCost = 10;

        // Resistances (reflecting the gown's mystical nature)
        Resistances.Cold = 15;
        Resistances.Energy = 10;

        // Skill Bonuses (thematically focused on magic and nature)
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);  // Boosts the caster’s spellcasting ability
        SkillBonuses.SetValues(1, SkillName.Mysticism, 15.0);  // Enhances ability to manipulate magical forces, like the Elves' deep connection with the arcane
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 10.0);  // Ties the gown’s magic to the Elven bond with nature and the world around them

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SylphshimmerGown(Serial serial) : base(serial)
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
