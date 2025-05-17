using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StrideOfTheJungleShade : TigerPeltLegs
{
    [Constructable]
    public StrideOfTheJungleShade()
    {
        Name = "Stride of the Jungle Shade";
        Hue = Utility.Random(2100, 2200); // Jungle-themed hue
        BaseArmorRating = Utility.RandomMinMax(12, 35); // Base armor rating for legs

        Attributes.BonusDex = 15; // Bonus to dexterity, aiding in speed and agility
        Attributes.BonusStam = 10; // Bonus to stamina, making the wearer more enduring
        Attributes.ReflectPhysical = 5; // Reflect physical damage back to attackers

        // Skill Bonuses: Focus on movement and stealth in nature
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0);
        SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0); // Affinity with nature

        ColdBonus = 5; // Cold resistance, useful in jungles with mist or night chill
        PhysicalBonus = 10; // Physical resistance, ideal for surviving rough terrain

        // Attach the item to the XML Level Item for any specific functionalities
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StrideOfTheJungleShade(Serial serial) : base(serial)
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
