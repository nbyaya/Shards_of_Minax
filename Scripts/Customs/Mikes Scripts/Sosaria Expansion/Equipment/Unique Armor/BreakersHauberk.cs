using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BreakersHauberk : StuddedDo
{
    [Constructable]
    public BreakersHauberk()
    {
        Name = "Breaker’s Hauberk";
        Hue = Utility.Random(2300, 2500);  // A dark, battle-worn hue.
        BaseArmorRating = Utility.RandomMinMax(35, 80);  // A solid defensive base value.

        // Attribute Bonuses
        Attributes.BonusStr = 15;
        Attributes.BonusDex = 10;
        Attributes.BonusHits = 25;
        Attributes.DefendChance = 10;
        Attributes.LowerRegCost = 10;
        Attributes.WeaponSpeed = 5;

        // Skill Bonuses - Thematically matching a heavy, battle-centric armor.
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Increase battle strategy effectiveness.
        SkillBonuses.SetValues(1, SkillName.Swords, 10.0);  // Strengthens swordsmanship for a warrior.
        SkillBonuses.SetValues(2, SkillName.Macing, 10.0);  // Adds power to blunt weapon skills.
        SkillBonuses.SetValues(3, SkillName.ArmsLore, 5.0);  // Reflects understanding of weaponry and armor.

        // Elemental Resistances - A well-rounded defensive piece.
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 10;
        PhysicalBonus = 20;
        PoisonBonus = 5;

        // Attach XmlLevelItem for leveling purposes.
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BreakersHauberk(Serial serial) : base(serial)
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
