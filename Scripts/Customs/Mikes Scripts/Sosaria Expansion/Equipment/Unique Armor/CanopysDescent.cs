using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CanopysDescent : LeafLegs
{
    [Constructable]
    public CanopysDescent()
    {
        Name = "The Canopy's Descent";
        Hue = Utility.Random(2000, 2300); // Leafy, forest tones
        BaseArmorRating = Utility.RandomMinMax(10, 30); // Lightweight, flexible protection

        Attributes.BonusDex = 15; // Agility-focused
        Attributes.BonusStam = 10; // Stamina boost for quick movement
        Attributes.DefendChance = 10; // Slightly improves defense chance to avoid detection
        Attributes.Luck = 10; // Small amount of luck, favorable for forest finds

        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0); // Enhances stealth abilities, perfect for moving unseen through the woods
        SkillBonuses.SetValues(1, SkillName.Tracking, 20.0); // Boosts ability to track animals and other players across the terrain
        SkillBonuses.SetValues(2, SkillName.Hiding, 15.0); // Enhances the ability to hide in natural environments

        ColdBonus = 5; // Minor cold resistance (forest weather)
        PhysicalBonus = 10; // Slight physical protection for a light armor

        XmlAttach.AttachTo(this, new XmlLevelItem()); // This attaches the item to the XML level system

    }

    public CanopysDescent(Serial serial) : base(serial)
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
