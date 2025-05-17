using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HatredOfTheSlumberingForge : HeavyPlateJingasa
{
    [Constructable]
    public HatredOfTheSlumberingForge()
    {
        Name = "Hatred of the Slumbering Forge";
        Hue = 1157;  // Fiery red hue, evoking a sense of anger and ancient wrath
        BaseArmorRating = 60;  // Solid protection, typical for a heavy plate item

        // Attributes: Focused on strength, defense, and regeneration to complement the heavy armor's function
        Attributes.BonusStr = 15;
        Attributes.BonusDex = 5;
        Attributes.BonusHits = 30;
        Attributes.DefendChance = 12;

        // Adding regeneration bonuses, symbolizing the armor's ancient, durable craftsmanship
        Attributes.RegenHits = 2;
        Attributes.RegenStam = 2;

        // Specific to armor's nature - providing offensive and defensive boosts
        Attributes.ReflectPhysical = 20;  // The armor reflects physical damage, symbolizing the forge's wrath

        // Skill bonuses: Tying together with the theme of ancient combat training and mastery in weaponry
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Offensive strategy and defense
        SkillBonuses.SetValues(1, SkillName.Swords, 10.0);   // Mastery with sword-based weapons, evoking combat styles forged in ancient times
        SkillBonuses.SetValues(2, SkillName.Mining, 10.0);   // A skill bonus tying to mining, a nod to the forging and crafting nature of the armor

        // Elemental resistance bonuses reflecting the armor's fiery and earthbound nature
        FireBonus = 15;
        PhysicalBonus = 10;

        // Attaching XML for level-based attributes, enhancing the uniqueness of this item
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HatredOfTheSlumberingForge(Serial serial) : base(serial)
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
