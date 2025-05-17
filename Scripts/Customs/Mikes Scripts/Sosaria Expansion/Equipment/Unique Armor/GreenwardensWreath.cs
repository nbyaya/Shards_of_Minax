using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GreenwardensWreath : LeafGorget
{
    [Constructable]
    public GreenwardensWreath()
    {
        Name = "Greenwarden's Wreath";
        Hue = 0x4A5; // A nature-inspired hue for the piece (greenish tone)
        BaseArmorRating = Utility.RandomMinMax(5, 15); // Low armor rating, fitting for a gorget

        Attributes.DefendChance = 10; // Offers some protection
        Attributes.BonusDex = 10; // Dexterity bonus, aiding agility for nature-based skills
        Attributes.BonusInt = 5; // Intelligence, fitting for nature-focused lore and magical potential
        Attributes.Luck = 10; // Luck bonus, representing the favor of the forest spirits

        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0); // Strengthening bond with animals
        SkillBonuses.SetValues(1, SkillName.Veterinary, 10.0); // Improving healing and care of creatures
        SkillBonuses.SetValues(2, SkillName.AnimalTaming, 10.0); // Boosting animal taming skills

        PhysicalBonus = 5; // Minor physical defense as the leaf itself provides light protection
        PoisonBonus = 5; // A minor defense against poison, as leaves have natural healing properties
        EnergyBonus = 5; // A little bit of energy protection, in tune with the natural elements

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach custom XML level item information
    }

    public GreenwardensWreath(Serial serial) : base(serial)
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
