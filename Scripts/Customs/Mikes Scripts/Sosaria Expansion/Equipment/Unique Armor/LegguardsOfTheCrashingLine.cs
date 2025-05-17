using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LegguardsOfTheCrashingLine : PlateLegs
{
    [Constructable]
    public LegguardsOfTheCrashingLine()
    {
        Name = "Legguards of the Crashing Line";
        Hue = Utility.Random(1150, 1300); // A battle-worn, metallic hue with hints of blue and grey
        BaseArmorRating = Utility.RandomMinMax(40, 60);

        // Defensive and Battle-related attributes
        Attributes.DefendChance = 10; // Increases defense against incoming attacks
        Attributes.BonusStr = 15; // Increases strength for increased melee damage and carrying capacity
        Attributes.BonusDex = 10; // Boosts dexterity, improving agility and attack speed
        Attributes.BonusStam = 15; // Boosts stamina for increased mobility and longer action duration
        Attributes.RegenStam = 5; // Regeneration of stamina over time

        // Skills that relate to the defensive and tactical theme of the item
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0); // Increases tactical knowledge
        SkillBonuses.SetValues(1, SkillName.Parry, 20.0); // Increases the ability to block or deflect attacks
        SkillBonuses.SetValues(2, SkillName.Wrestling, 15.0); // Increases unarmed combat abilities

        // Elemental resistances based on the thematic origin of the armor (battle-tested)
        FireBonus = 10; // Slight resistance to fire, from the battlefields
        PhysicalBonus = 20; // Increased physical resistance for the armor's durability in combat

        // Special bonus for tactical combat (the Crashing Line)
        Attributes.LowerManaCost = 5; // Reduces mana costs for abilities that involve combat tactics

        // Attach XmlLevelItem to allow for scaling and progression
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LegguardsOfTheCrashingLine(Serial serial) : base(serial)
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
