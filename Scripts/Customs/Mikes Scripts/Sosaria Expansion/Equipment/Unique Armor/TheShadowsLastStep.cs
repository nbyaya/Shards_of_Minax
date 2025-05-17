using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheShadowsLastStep : LeatherNinjaPants
{
    [Constructable]
    public TheShadowsLastStep()
    {
        Name = "The Shadow's Last Step";
        Hue = Utility.Random(1, 1000); // Random color for the pants' appearance
        BaseArmorRating = Utility.RandomMinMax(10, 35); // Lightweight, suitable for stealth

        // Adding attributes that fit the ninja's elusive nature
        Attributes.BonusDex = 15; // Dexterity bonus for agility and stealth
        Attributes.BonusStam = 10; // Increases stamina for swift movements
        Attributes.DefendChance = 10; // Slightly better chance to avoid attacks
        Attributes.LowerManaCost = 5; // Reduces mana cost, useful for ninjitsu
        Attributes.ReflectPhysical = 5; // Minor physical damage reflection

        // Skills that complement a stealthy, elusive ninja archetype
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0); // Enhanced Stealth skill
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 20.0); // Increases Ninjitsu effectiveness
        SkillBonuses.SetValues(2, SkillName.Hiding, 15.0); // Boosts the ability to hide

        // Elemental resistances to keep the ninja protected from certain dangers
        ColdBonus = 10; 
        FireBonus = 10;

        // Attach XmlLevelItem to this item for potential level-based progression
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheShadowsLastStep(Serial serial) : base(serial)
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
