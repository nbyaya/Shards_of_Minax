using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StormweldBracers : PlateArms
{
    [Constructable]
    public StormweldBracers()
    {
        Name = "Stormweld Bracers";
        Hue = 1157;  // A stormy blue, to match the "storm" theme
        BaseArmorRating = Utility.RandomMinMax(35, 75);  // A solid protection value for arms

        ArmorAttributes.SelfRepair = 10;  // Durable armor, self-repairing
        Attributes.BonusStr = 10;  // Strength boost for combat
        Attributes.DefendChance = 5;  // Increases defense chance slightly
        Attributes.SpellDamage = 10;  // Channeling some of the storm's power
        Attributes.RegenStam = 3;  // Helps with stamina regen in the heat of battle
        Attributes.ReflectPhysical = 5;  // Reflects some of the physical damage back to attackers

        // Skill Bonuses: Thematically connected to combat and storm-based effects
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Expertise in combat tactics
        SkillBonuses.SetValues(1, SkillName.Swords, 10.0);  // Boosts sword fighting ability
        SkillBonuses.SetValues(2, SkillName.Parry, 10.0);  // Enhances defensive abilities in battle

        ColdBonus = 10;  // Resistance to cold, perhaps hinting at the storm's chill
        FireBonus = 10;  // And fire resistance, countering the storm's volatility
        PhysicalBonus = 5;  // Resilience against physical damage

        XmlAttach.AttachTo(this, new XmlLevelItem());  // Ensure it's linked to the level system
    }

    public StormweldBracers(Serial serial) : base(serial)
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
