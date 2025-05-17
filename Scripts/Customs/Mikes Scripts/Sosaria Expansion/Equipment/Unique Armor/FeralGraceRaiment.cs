using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FeralGraceRaiment : HideFemaleChest
{
    [Constructable]
    public FeralGraceRaiment()
    {
        Name = "Feral Grace Raiment";
        Hue = Utility.Random(1, 1000); // Assign a randomized hue to make it unique
        BaseArmorRating = Utility.RandomMinMax(25, 55); // Appropriate armor rating for this type

        // Attribute bonuses enhancing agility and survival skills
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.DefendChance = 10;
        Attributes.ReflectPhysical = 5;

        // Skill bonuses that synergize with the theme of animal lore and survival
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0); // Enhances knowledge about animals
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0); // Improves tracking abilities
        SkillBonuses.SetValues(2, SkillName.Swords, 5.0); // Bonus to swordplay, fitting for agile, feral combat

        // Elemental resistances focused on natural protection
        ColdBonus = 10;
        PhysicalBonus = 15;

        // Attach custom XML Level Item to make it persist with special attributes
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FeralGraceRaiment(Serial serial) : base(serial)
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
