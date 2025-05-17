using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VerdantEmbrace : LeafChest
{
    [Constructable]
    public VerdantEmbrace()
    {
        Name = "Verdant Embrace";
        Hue = 0x3F2;  // Greenish tint to symbolize nature
        BaseArmorRating = Utility.RandomMinMax(30, 50);  // Reasonable base armor for a nature-themed item

        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 10;
        Attributes.RegenStam = 3;
        Attributes.RegenMana = 3;
        Attributes.LowerManaCost = 10;
        
        // Thematically tied to nature and the protection of the forest
        SkillBonuses.SetValues(0, SkillName.Veterinary, 20.0);  // Bond with animals, nature skills
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);  // Healing, connected to nature's restorative powers
        SkillBonuses.SetValues(2, SkillName.Herding, 15.0);  // Managing the wilderness and animal control

        ColdBonus = 5;  // Protection against cold, as the forest can be chilly
        PoisonBonus = 10;  // Strength against poison, symbolizing the natural defense mechanisms of the forest

        // Custom effects to fit the nature theme
        Attributes.DefendChance = 10;  // The wearer is attuned to nature, evading attacks
        ArmorAttributes.SelfRepair = 5;  // Natural regeneration in the wild

        XmlAttach.AttachTo(this, new XmlLevelItem());  // Ensure the item is linked to a level-based item system
    }

    public VerdantEmbrace(Serial serial) : base(serial)
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
