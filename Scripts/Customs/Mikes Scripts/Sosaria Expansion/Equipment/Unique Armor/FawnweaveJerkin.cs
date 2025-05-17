using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FawnweaveJerkin : FemaleLeatherChest
{
    [Constructable]
    public FawnweaveJerkin()
    {
        Name = "Fawnweave Jerkin";
        Hue = Utility.Random(2000, 2300);  // Earthy tones, blending into nature
        BaseArmorRating = Utility.RandomMinMax(15, 40);

        // Set Attributes
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.DefendChance = 5;

        // Skill Bonuses: These are nature-related skills to fit with the 'Fawnweave' theme
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);  // Connection to nature and creatures
        SkillBonuses.SetValues(1, SkillName.Hiding, 15.0);  // Stealth and blending into nature
        SkillBonuses.SetValues(2, SkillName.Tracking, 10.0);  // Tied to understanding animal movements and the land

        // Elemental bonuses that relate to nature’s resilience
        PhysicalBonus = 10;
        ColdBonus = 5;
        PoisonBonus = 10;

        // Additional effects to enhance the player’s interaction with the natural world
        Attributes.LowerManaCost = 5;  // Favors magic users who seek communion with nature
        ArmorAttributes.SelfRepair = 5;  // A blessing of nature that slowly repairs itself over time

        // Attach the item to the XmlLevel system
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FawnweaveJerkin(Serial serial) : base(serial)
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
