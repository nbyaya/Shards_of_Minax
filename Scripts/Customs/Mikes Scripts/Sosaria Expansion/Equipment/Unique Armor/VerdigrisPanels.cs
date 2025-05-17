using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VerdigrisPanels : LeafTonlet
{
    [Constructable]
    public VerdigrisPanels()
    {
        Name = "Verdigris Panels";
        Hue = 0x96D;  // A color reminiscent of verdant foliage and natural tones
        BaseArmorRating = Utility.RandomMinMax(30, 50);

        // Attributes related to defense and agility, reflecting the connection to nature
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.DefendChance = 10;
        Attributes.Luck = 50;
        
        // Reflective of the woodland aspect, enhancing the ability to blend into natural surroundings
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
        SkillBonuses.SetValues(2, SkillName.Hiding, 20.0);

        // Bonus resistances to elements related to the natural world
        ColdBonus = 10;
        PoisonBonus = 10;

        // Adding a unique visual or effect, such as "self-repair" for a mystical, nature-infused feel
        ArmorAttributes.SelfRepair = 5;

        // Attach the item to an XML level system
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VerdigrisPanels(Serial serial) : base(serial)
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
