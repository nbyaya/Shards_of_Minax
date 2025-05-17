using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KnucklebindsOfTheThornedPath : StuddedArms
{
    [Constructable]
    public KnucklebindsOfTheThornedPath()
    {
        Name = "Knucklebinds of the Thorned Path";
        Hue = Utility.Random(0, 1000);  // A random hue to give it a unique appearance.
        BaseArmorRating = Utility.RandomMinMax(15, 45);  // Set armor rating to reflect light but protective nature.

        // Adding thematic attributes related to agility, stealth, and nature.
        Attributes.BonusDex = 15;  // Enhancing Dexterity to improve agility.
        Attributes.BonusStam = 10;  // Bonus Stamina for more endurance while sneaking or avoiding danger.
        Attributes.DefendChance = 10;  // Increase chance of evading attacks.
        Attributes.LowerManaCost = 5;  // A small reduction in mana cost for quick spellcasting when needed.

        // Skill bonuses to fit the "Thorned Path" theme (stealthy and dangerous, with a nature connection).
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);  // Enhancing Stealth for sneaking.
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);  // Improving combat effectiveness.
        SkillBonuses.SetValues(2, SkillName.Poisoning, 10.0);  // Enhancing Poisoning skills for a deadly touch.

        // Minor elemental bonuses (suggesting a natural affinity with the land, yet a dangerous one).
        ColdBonus = 5;
        PoisonBonus = 15;

        // Attach this item to XmlLevelItem for consistency.
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KnucklebindsOfTheThornedPath(Serial serial) : base(serial)
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
