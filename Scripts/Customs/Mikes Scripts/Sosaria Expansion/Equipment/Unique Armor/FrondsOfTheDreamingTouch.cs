using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrondsOfTheDreamingTouch : LeafGloves
{
    [Constructable]
    public FrondsOfTheDreamingTouch()
    {
        Name = "Fronds of the Dreaming Touch";
        Hue = 0x1B74; // Greenish hue to match the nature theme
        BaseArmorRating = 10; // A modest armor rating to keep with its nature-inspired theme

        // Attributes related to agility, nature, and mysticism
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 10;
        Attributes.RegenStam = 2;
        Attributes.CastSpeed = 1;
        Attributes.LowerManaCost = 5;
        Attributes.NightSight = 1; // To reflect the ethereal connection to the moon and nature

        // Skill bonuses for Nature, Stealth, and Healing
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0); // Connecting to the lore and creatures of nature
        SkillBonuses.SetValues(1, SkillName.Healing, 10.0); // Reflecting the healing touch of nature
        SkillBonuses.SetValues(2, SkillName.Stealth, 10.0); // Natural stealth, moving unseen like a leaf in the wind

        // Elemental resistances, emphasizing the harmony with nature
        PhysicalBonus = 5; 
        PoisonBonus = 15; // Nature’s resilience to poison, given its connection to the earth

        // XmlLevelItem for custom leveling functionality
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrondsOfTheDreamingTouch(Serial serial) : base(serial)
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
