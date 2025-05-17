using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThroatOfTheWildBond : LeatherGorget
{
    [Constructable]
    public ThroatOfTheWildBond()
    {
        Name = "Throat of the Wild Bond";
        Hue = 1102; // A natural green hue, blending with the wilderness.
        BaseArmorRating = Utility.RandomMinMax(15, 40); // Balanced for a gorget.
        
        // Attributes that enhance survival and connection to the wild
        Attributes.BonusInt = 10;
        Attributes.BonusDex = 5;
        Attributes.BonusHits = 10;
        Attributes.ReflectPhysical = 5; // Reflect a small amount of damage from physical attacks

        // Skill bonuses centered around animals and wilderness
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0); // Enhances knowledge of animals.
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 10.0); // Boosts Animal Taming abilities.
        SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0); // Enhances animal healing and care.

        // Environmental resistances to aid survival in the wild
        PhysicalBonus = 10;
        PoisonBonus = 5;

        // Add unique XML attach for additional functionality
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThroatOfTheWildBond(Serial serial) : base(serial)
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
