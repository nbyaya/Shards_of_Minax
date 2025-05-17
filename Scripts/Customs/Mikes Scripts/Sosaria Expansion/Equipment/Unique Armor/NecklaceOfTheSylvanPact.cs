using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NecklaceOfTheSylvanPact : WoodlandGorget
{
    [Constructable]
    public NecklaceOfTheSylvanPact()
    {
        Name = "Necklace of the Sylvan Pact";
        Hue = Utility.Random(1001, 1100);  // Green hues, fitting for woodland themes
        Weight = 1.0;

        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 5;
        Attributes.DefendChance = 10;
        Attributes.NightSight = 1;

        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0);  // Enhances animal-related skills
        SkillBonuses.SetValues(1, SkillName.Veterinary, 10.0);  // Animal healing / care
        SkillBonuses.SetValues(2, SkillName.Herding, 10.0);  // Allows the player to manage animals more effectively

        ColdBonus = 10;  // A little cold resistance, as the forest can be chilly
        EnergyBonus = 5; // A touch of magical protection
        PhysicalBonus = 5;  // General protection

        XmlAttach.AttachTo(this, new XmlLevelItem());  // Ensures this item is recognized as a unique item in the game

    }

    public NecklaceOfTheSylvanPact(Serial serial) : base(serial)
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
