using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LatticeOfTheFallenLegion : StuddedHiroSode
{
    [Constructable]
    public LatticeOfTheFallenLegion()
    {
        Name = "Lattice of the Fallen Legion";
        Hue = Utility.Random(1150, 1250); // A dark, warrior-like hue, possibly with hints of crimson or gold.
        BaseArmorRating = Utility.RandomMinMax(30, 75); // A solid defensive rating for a rare, durable armor.

        ArmorAttributes.SelfRepair = 15; // A reminder of the fallen legion’s enduring nature.
        Attributes.BonusStr = 10; // Strength to symbolize resilience and battle readiness.
        Attributes.BonusDex = 5; // Dexterity for agility in battle.
        Attributes.BonusInt = 5; // A slight boost to represent the strategic minds of fallen soldiers.
        Attributes.RegenHits = 3; // The armor's legacy aids the wearer in recovering health during battle.

        // Skill Bonuses: These skills represent the combat prowess and ancient tactics of the Legion.
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // The wearer's combat strategy improves.
        SkillBonuses.SetValues(1, SkillName.Swords, 10.0); // Strengthens the wearer's swordsmanship.
        SkillBonuses.SetValues(2, SkillName.Parry, 10.0); // Boosts defensive capability, reflecting the armor’s tactical nature.

        // Elemental resistances, to reflect its long-standing heritage and fortitude.
        ColdBonus = 10; // Reflecting a hardened resistance.
        FireBonus = 10; // The armor has withstood the test of time and fire.
        PhysicalBonus = 15; // An additional resistance as part of its combat legacy.

        // XmlLevelItem attachment for progression-based item system.
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LatticeOfTheFallenLegion(Serial serial) : base(serial)
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
