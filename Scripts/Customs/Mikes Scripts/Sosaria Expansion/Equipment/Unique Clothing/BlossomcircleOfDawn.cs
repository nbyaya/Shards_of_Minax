using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlossomcircleOfDawn : FlowerGarland
{
    [Constructable]
    public BlossomcircleOfDawn()
    {
        Name = "Blossomcircle of Dawn";
        Hue = Utility.Random(1, 3000); // Soft floral hues like pale pinks and greens

        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 5;
        Attributes.BonusInt = 15;
        Attributes.BonusMana = 20;
        Attributes.RegenMana = 3;
        Attributes.Luck = 50;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 10.0); // Animal affinity, fitting for a flower-themed item
        SkillBonuses.SetValues(1, SkillName.Healing, 10.0); // Connection to nature's healing
        SkillBonuses.SetValues(2, SkillName.Cooking, 10.0); // Subtle link to the nurturing aspects of nature

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlossomcircleOfDawn(Serial serial) : base(serial)
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
