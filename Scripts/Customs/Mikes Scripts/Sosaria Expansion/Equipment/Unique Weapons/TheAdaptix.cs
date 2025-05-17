using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheAdaptix : TacticalMultitool
{
    [Constructable]
    public TheAdaptix()
    {
        Name = "The Adaptix";
        Hue = Utility.Random(2300, 2400);  // A sleek, metallic hue representing versatility and innovation
        MinDamage = Utility.RandomMinMax(15, 35);
        MaxDamage = Utility.RandomMinMax(40, 60);

        Attributes.WeaponSpeed = 5;  // Optimized for rapid utility use in combat
        Attributes.Luck = 10;
        Attributes.BonusStr = 5;  // Enhances physical strength for adaptability in battle
        Attributes.BonusDex = 10; // Boosts agility, crucial for using the multitool’s various functions
        Attributes.BonusInt = 5;  // Aids in manipulating various components of the multitool’s magic enhancements

        // Special Function: Adaptix is equipped with an assortment of tools, ideal for strategic combat and survival
        WeaponAttributes.HitDispel = 30;  // Dispel magic to counter hostile enchantments
        WeaponAttributes.HitLowerDefend = 15;  // Weakens enemy defense, making them vulnerable to subsequent attacks
        WeaponAttributes.HitLeechHits = 20;  // Restores health to the wielder, an essential function for prolonged survival

        // Slayer effect – The Adaptix excels against enemies known for their adaptability or toughness
        Slayer = SlayerName.ElementalBan; // Especially effective against elementals, enhancing its adaptability in diverse environments

        // Skill bonuses related to its tactical utility in both combat and survival scenarios
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);  // Perfect for optimizing tactical positioning and strategy
        SkillBonuses.SetValues(1, SkillName.Mysticism, 15.0);  // Boosts the ability to manipulate magical forces or artifacts
        SkillBonuses.SetValues(2, SkillName.Tinkering, 20.0);  // Enhances the skill to manipulate mechanical components, fitting the multitool theme
        SkillBonuses.SetValues(3, SkillName.Lockpicking, 15.0); // Useful for bypassing obstacles or unlocking strategic paths

        // Additional thematic bonus for versatility in combat, granting an edge in multiple situations
        Attributes.RegenHits = 5;  // Helps recover health more efficiently after each engagement
        Attributes.RegenStam = 5;  // Aids stamina regeneration for prolonged use of the multitool

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheAdaptix(Serial serial) : base(serial)
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
