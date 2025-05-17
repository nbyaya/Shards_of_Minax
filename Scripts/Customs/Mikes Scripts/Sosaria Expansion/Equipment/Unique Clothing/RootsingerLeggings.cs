using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RootsingerLeggings : ElvenPants
{
    [Constructable]
    public RootsingerLeggings()
    {
        Name = "Rootsinger Leggings";
        Hue = 1350;  // Green, earthy color to match the elven nature theme
        
        // Set attributes and bonuses
        Attributes.BonusDex = 12;
        Attributes.BonusInt = 8;
        Attributes.BonusHits = 10;
        Attributes.BonusStam = 12;


        // Resistances
        Resistances.Physical = 12;
        Resistances.Poison = 18;
        Resistances.Cold = 8;
        Resistances.Energy = 6;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 10.0);  // Elven bond with nature, understanding of animals
        SkillBonuses.SetValues(1, SkillName.Healing, 8.0);  // Elven affinity with restoration and nature's healing power
        SkillBonuses.SetValues(2, SkillName.AnimalTaming, 12.0);  // Nature's protector role and bond with beasts
        SkillBonuses.SetValues(3, SkillName.Veterinary, 10.0);  // Expertise in animal care
        SkillBonuses.SetValues(4, SkillName.Mysticism, 5.0);  // Connection to the magical energies of the forest

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RootsingerLeggings(Serial serial) : base(serial)
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
