using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MantleOfBeastsWill : HideChest
{
    [Constructable]
    public MantleOfBeastsWill()
    {
        Name = "Mantle of Beast's Will";
        Hue = Utility.Random(900, 1200); // Earthy tones, like greens and browns
        BaseArmorRating = Utility.RandomMinMax(35, 60);

        ArmorAttributes.SelfRepair = 5; // This mantle endures through the wear and tear of the wild
        Attributes.BonusStr = 15; // Strength of beasts
        Attributes.BonusDex = 10; // Agility of creatures in the wild
        Attributes.BonusHits = 25; // Stamina drawn from nature's resilience

        // Skill Bonuses related to animal behavior and nature:
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0); // Understanding the beasts
        SkillBonuses.SetValues(1, SkillName.Veterinary, 15.0); // Healing animals
        SkillBonuses.SetValues(2, SkillName.Tracking, 10.0); // Following wild paths

        PhysicalBonus = 20; // Extra protection, as the mantle enhances physical defense
        PoisonBonus = 10; // Enhanced resistance against poisons from nature's venomous creatures

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Allows integration with the XML Level system
    }

    public MantleOfBeastsWill(Serial serial) : base(serial)
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
