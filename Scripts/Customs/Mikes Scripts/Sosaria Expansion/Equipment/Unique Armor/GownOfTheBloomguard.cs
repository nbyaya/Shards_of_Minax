using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GownOfTheBloomguard : FemaleLeafChest
{
    [Constructable]
    public GownOfTheBloomguard()
    {
        Name = "Gown of the Bloomguard";
        Hue = Utility.Random(1, 1000); // Random hue for variety
        BaseArmorRating = Utility.RandomMinMax(25, 50); // Adjust armor rating as needed

        Attributes.BonusInt = 15; // Boosts intelligence
        Attributes.BonusDex = 10; // Boosts dexterity, reflecting the agility of the forest
        Attributes.BonusHits = 20; // Provides extra health, reflecting the vitality of nature
        Attributes.RegenHits = 5; // Helps to regenerate health faster in natural environments
        Attributes.RegenStam = 4; // Improves stamina regeneration, aiding in quick reflexes and agility
        Attributes.DefendChance = 10; // Increases defense, representing the defensive nature of the forest spirits

        SkillBonuses.SetValues(0, SkillName.Veterinary, 15.0); // Ties to nature, aiding in animal care
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0); // Understanding of animals and wildlife
        SkillBonuses.SetValues(2, SkillName.AnimalTaming, 10.0); // Increases animal taming abilities, a natural affinity with creatures

        ColdBonus = 10; // Light protection against cold, natural adaptation to the elements
        EnergyBonus = 5; // Energy bonus to reflect the earthy, magical properties of the gown
        PhysicalBonus = 10; // Adds physical resistance to give the wearer strength against physical attacks

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach unique properties for the item

    }

    public GownOfTheBloomguard(Serial serial) : base(serial)
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
