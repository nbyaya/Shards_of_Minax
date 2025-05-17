using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SkirtOfTheCrescentRoar : TigerPeltSkirt
{
    [Constructable]
    public SkirtOfTheCrescentRoar()
    {
        Name = "Skirt of the Crescent Roar";
        Hue = 1141; // Tiger-like pelt color
        BaseArmorRating = Utility.RandomMinMax(8, 25); // Base AR for skirts
        ArmorAttributes.SelfRepair = 5;

        Attributes.BonusStr = 10;
        Attributes.BonusDex = 10;
        Attributes.BonusHits = 20;
        Attributes.DefendChance = 5;
        Attributes.LowerManaCost = 5;
        Attributes.EnhancePotions = 10;

        // Skill Bonuses related to animal lore, tracking, and wilderness
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0);
        SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);
        SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0);

        ColdBonus = 10;
        PhysicalBonus = 10;

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach to XML level item system
    }

    public SkirtOfTheCrescentRoar(Serial serial) : base(serial)
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
