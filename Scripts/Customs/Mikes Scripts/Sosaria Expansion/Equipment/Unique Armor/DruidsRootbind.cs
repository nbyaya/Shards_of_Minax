using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DruidsRootbind : HideGorget
{
    [Constructable]
    public DruidsRootbind()
    {
        Name = "Druid's Rootbind";
        Hue = Utility.Random(1, 1000); // Greenish hues to reflect its connection to nature.
        BaseArmorRating = Utility.RandomMinMax(10, 50); // Base armor rating is modest, fitting for a druidic item.

        Attributes.BonusStr = 5;  // The strength to carry the weight of nature's burden.
        Attributes.BonusDex = 10; // Dexterity for better survival in the wild.
        Attributes.BonusInt = 15; // Intellect, for wisdom in nature's magic.

        Attributes.RegenHits = 3; // Slight regeneration, a touch of nature's healing.
        Attributes.RegenStam = 2; // Stamina regeneration, for long journeys through the forest.
        Attributes.LowerManaCost = 5; // Reduces mana costs, useful for casting nature-based spells.

        SkillBonuses.SetValues(0, SkillName.AnimalLore, 10.0);  // Tied to animal lore to better understand creatures of the forest.
        SkillBonuses.SetValues(1, SkillName.Veterinary, 10.0);  // Veterinary to heal animals and creatures of the wild.
        SkillBonuses.SetValues(2, SkillName.AnimalTaming, 5.0); // For the druid's ability to tame and communicate with beasts.

        PhysicalBonus = 5;  // Provides some physical protection against the elements.
        PoisonBonus = 5; // Protection from toxins in the wild.
        
        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach to the XML level item system.

    }

    public DruidsRootbind(Serial serial) : base(serial)
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
