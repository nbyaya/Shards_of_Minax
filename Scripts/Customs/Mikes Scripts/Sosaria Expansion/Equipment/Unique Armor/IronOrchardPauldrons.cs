using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class IronOrchardPauldrons : PlateHiroSode
{
    [Constructable]
    public IronOrchardPauldrons()
    {
        Name = "Iron Orchard Pauldrons";
        Hue = Utility.Random(1, 1000); // Random color hue
        BaseArmorRating = Utility.RandomMinMax(40, 60); // Random AR for balance

        ArmorAttributes.SelfRepair = 5; // Armor can repair itself
        Attributes.BonusStr = 15; // Bonus to Strength
        Attributes.BonusDex = 10; // Bonus to Dexterity
        Attributes.DefendChance = 10; // Increased defense chance
        Attributes.Luck = 20; // Luck attribute for increased drops or bonuses
        Attributes.RegenHits = 3; // Bonus regeneration to health
        Attributes.RegenStam = 2; // Bonus regeneration to stamina

        // Skill Bonuses that tie into the nature of "Iron Orchard" theme (strength and tactics)
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // Boosts the tactics skill
        SkillBonuses.SetValues(1, SkillName.Swords, 10.0);  // Boosts the swordsmanship skill
        SkillBonuses.SetValues(2, SkillName.ArmsLore, 10.0); // Boosts arms lore skill, fitting with armor expertise

        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 10; // Elements to balance resistances
        PhysicalBonus = 15;
        PoisonBonus = 5;

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach the XML level item tag
    }

    public IronOrchardPauldrons(Serial serial) : base(serial)
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
