using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FortressBornShinplates : PlateHaidate
{
    [Constructable]
    public FortressBornShinplates()
    {
        Name = "Fortress-Born Shinplates";
        Hue = Utility.Random(1000, 1500);  // A dark, earthy tone to reflect the fortress theme.
        BaseArmorRating = Utility.RandomMinMax(40, 70); // High defensive armor rating for shinplates.

        // Armor Attributes
        ArmorAttributes.SelfRepair = 5;  // Basic self-repair for durability.
        Attributes.DefendChance = 10;  // Increases the chance to defend against attacks.
        Attributes.BonusStam = 15;  // Boosts stamina, essential for long battles.
        Attributes.BonusHits = 25;  // Increases health, enhancing overall durability.
        
        // Skill Bonuses: Thematically focused on defense and martial combat.
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Enhances combat tactics and strategy.
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);    // Boosts parrying ability, a key defense skill.
        SkillBonuses.SetValues(2, SkillName.Swords, 10.0);   // Reflects the combat strength of warriors.

        // Elemental Damage Resistance (defensive, thematic to fortress-like protection)
        PhysicalBonus = 25; // Increased physical damage resistance.
        FireBonus = 5;      // Small resistance to fire damage.
        PoisonBonus = 5;    // Small resistance to poison damage.

        // Attach the unique item behavior (XML-level item attributes)
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FortressBornShinplates(Serial serial) : base(serial)
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
