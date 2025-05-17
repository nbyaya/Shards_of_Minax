using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ScaleborneWyrmplate : DragonChest
{
    [Constructable]
    public ScaleborneWyrmplate()
    {
        Name = "Scaleborne Wyrmplate";
        Hue = 1150;  // A fiery red or golden hue to represent dragon scales.
        BaseArmorRating = Utility.RandomMinMax(45, 100);

        // Attributes reflecting dragon-related bonuses and power
        Attributes.BonusStr = 15;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 5;
        Attributes.DefendChance = 10;
        Attributes.LowerManaCost = 10;
        Attributes.Luck = 50;
        Attributes.ReflectPhysical = 20;
        Attributes.RegenHits = 2;
        Attributes.RegenStam = 2;
        Attributes.SpellDamage = 10;

        // Bonus to resistances to align with dragon lore
        FireBonus = 20;
        PhysicalBonus = 15;
        EnergyBonus = 5;
        
        // Skill bonuses relating to the draconic nature, warfare, and resilience
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Macing, 15.0);  // For combat with heavy armor
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0); // Understanding physical resilience

        // Attach XmlLevelItem for further customization and use in the world
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ScaleborneWyrmplate(Serial serial) : base(serial)
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
