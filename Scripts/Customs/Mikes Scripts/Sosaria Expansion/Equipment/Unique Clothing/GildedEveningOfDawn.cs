using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GildedEveningOfDawn : FancyDress
{
    [Constructable]
    public GildedEveningOfDawn()
    {
        Name = "Gilded Evening of Dawn";
        Hue = 1368;  // A shimmering golden hue
        ItemID = 0x1F04; // Elegant, fancy dress graphic

        // Set attributes and bonuses
        Attributes.BonusDex = 8;
        Attributes.BonusInt = 15;
        Attributes.BonusHits = 30;
        Attributes.BonusMana = 25;
        Attributes.CastSpeed = 1;  // Magical enhancement for casting
        Attributes.LowerManaCost = 10;
        Attributes.Luck = 50; // Luck bonus to reflect its rarity

        // Resistances
        Resistances.Fire = 15;
        Resistances.Cold = 10;
        Resistances.Poison = 10;
        Resistances.Energy = 10;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Magery, 10.0); // A magical touch for the wearer
        SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0); // Knowledge-based skills to match its regal nature
        SkillBonuses.SetValues(2, SkillName.Healing, 5.0); // The dress subtly boosts healing potential
        SkillBonuses.SetValues(3, SkillName.Meditation, 10.0); // Perfect for calming one's mind in the midst of chaos
        SkillBonuses.SetValues(4, SkillName.Spellweaving, 5.0); // Adds a hint of ancient arcane power

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GildedEveningOfDawn(Serial serial) : base(serial)
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
