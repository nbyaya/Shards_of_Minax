using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HuntingQueensTrail : TigerPeltLongSkirt
{
    [Constructable]
    public HuntingQueensTrail()
    {
        Name = "Hunting Queen's Trail";
        Hue = Utility.Random(2000, 2500);  // Earthy tones with some regal highlights
        BaseArmorRating = Utility.RandomMinMax(25, 65); // Moderate armor rating

        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 5;
        Attributes.BonusHits = 10;
        Attributes.RegenStam = 3;
        Attributes.DefendChance = 10;

        SkillBonuses.SetValues(0, SkillName.Tracking, 20.0); // Boost to tracking skill
        SkillBonuses.SetValues(1, SkillName.Veterinary, 15.0); // Boost to veterinary for animal care
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 20.0); // Enhances knowledge of animals, essential for hunting

        ColdBonus = 10;  // Useful for colder regions where hunting may be more common
        PhysicalBonus = 15; // Physical defense for hunting in dangerous environments

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Ensures the item is treated as a level item in XML

    }

    public HuntingQueensTrail(Serial serial) : base(serial)
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
