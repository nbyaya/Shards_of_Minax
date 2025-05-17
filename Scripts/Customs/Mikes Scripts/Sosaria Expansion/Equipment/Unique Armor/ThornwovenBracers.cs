using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThornwovenBracers : WoodlandGloves
{
    [Constructable]
    public ThornwovenBracers()
    {
        Name = "Thornwoven Bracers";
        Hue = Utility.Random(1000, 1500); // A deep greenish-brown hue, like the forest floor.
        BaseArmorRating = Utility.RandomMinMax(5, 20);

        // Nature-themed attributes
        Attributes.BonusDex = 10;  // Boosts dexterity, to reflect the agility of a forest dweller.
        Attributes.BonusStam = 15; // Increases stamina to aid in mobility and endurance.
        Attributes.RegenStam = 2;  // Small stamina regeneration, like the forest’s slow healing process.

        // Skills that tie into wilderness and nature
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0);  // Ties into nature knowledge, perfect for a character connected with the wild.
        SkillBonuses.SetValues(1, SkillName.Veterinary, 10.0);  // Works with animals and creatures, emphasizing the druidic connection.
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);  // Adds some combat utility, as the bracers could have some defensive purpose.

        // Additional bonuses related to the environment or mystical attributes
        PoisonBonus = 5;  // The thorns have a slight venomous edge, adding a poison defense element.
        PhysicalBonus = 10;  // Offers more protection, like bark armor.

        // Attach a special XML level item so it can be enhanced in the game world.
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThornwovenBracers(Serial serial) : base(serial)
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
