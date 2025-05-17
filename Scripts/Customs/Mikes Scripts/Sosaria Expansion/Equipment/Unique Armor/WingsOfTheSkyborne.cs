using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WingsOfTheSkyborne : DragonArms
{
    [Constructable]
    public WingsOfTheSkyborne()
    {
        Name = "Wings of the Skyborne";
        Hue = Utility.Random(1150, 1200); // A hue fitting for dragon scales, fiery or sky-blue.
        BaseArmorRating = Utility.RandomMinMax(30, 70); // A solid protection rating, reflecting dragon-like resilience.

        ArmorAttributes.SelfRepair = 5; // Self-repairing like dragon scales, natural resilience.
        
        Attributes.BonusStr = 15; // The strength of the dragon enhances the wearer.
        Attributes.BonusDex = 10; // Dexterity and agility to emulate the grace of flight.
        Attributes.BonusInt = 5; // Small intelligence bonus to reflect the cunning of dragons.

        // Relevant skills that align with dragon-themed powers:
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0); // Fencing is often associated with agile combat, like dragon claws.
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0); // Dragons are known for their resistance to magical influences.
        SkillBonuses.SetValues(2, SkillName.Tactics, 15.0); // Strategic thinking and combat prowess.

        // Elemental bonuses, reflecting the power of a dragon's breath or elemental influence.
        FireBonus = 15; // The fiery nature of dragons.
        PhysicalBonus = 10; // Physical defense against physical attacks.

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach XML level item for additional features.

    }

    public WingsOfTheSkyborne(Serial serial) : base(serial)
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
