using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PeltknitBreeches : TigerPeltShorts
{
    [Constructable]
    public PeltknitBreeches()
    {
        Name = "Peltknit Breeches";
        Hue = 0x48C; // A tigerish brown color
        BaseArmorRating = Utility.RandomMinMax(10, 40); // Light armor value for shorts

        Attributes.BonusDex = 10; // Increased dexterity for stealth and agility
        Attributes.BonusStr = 5; // Slight strength boost, useful for combat and survival
        Attributes.DefendChance = 8; // Defend chance, fitting for light armor that focuses on evasion
        Attributes.Luck = 15; // A little extra luck, symbolizing the tiger's cunning and stealth

        SkillBonuses.SetValues(0, SkillName.AnimalLore, 10.0); // Helps with taming or understanding wild creatures
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0); // Boosts stealth, making the wearer more elusive, like a tiger in the wild
        SkillBonuses.SetValues(2, SkillName.Tracking, 10.0); // Useful for following creatures in the wild, perfect for a hunter or scout

        ColdBonus = 5; // A slight resistance to cold, perhaps due to the protective nature of the pelt
        PhysicalBonus = 5; // Resistance to physical damage, as the tiger pelt offers some protection

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PeltknitBreeches(Serial serial) : base(serial)
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
