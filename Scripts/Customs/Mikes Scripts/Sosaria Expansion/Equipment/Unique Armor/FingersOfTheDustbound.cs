using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FingersOfTheDustbound : BoneGloves
{
    [Constructable]
    public FingersOfTheDustbound()
    {
        Name = "Fingers of the Dustbound";
        Hue = Utility.Random(2200, 2300); // A dusty, worn look
        BaseArmorRating = Utility.RandomMinMax(12, 40); // Lower armor rating but high utility

        // Attribute modifications
        Attributes.BonusInt = 15;
        Attributes.BonusMana = 20;
        Attributes.LowerManaCost = 10;  // Necromancy often uses a lot of mana
        Attributes.DefendChance = 10;   // Adds some defense for survivability

        // Skill bonuses that complement necromancy and anatomy
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 20.0);  // SpiritSpeak is crucial for Necromancy

        // Elemental bonuses to give it a sense of mystical power
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 5;  // The gloves are linked to the power of spirits, and Fire can represent the unsettling energy of the dead
        PhysicalBonus = 5;
        PoisonBonus = 10;

        // Attach the XML for item-level attributes
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FingersOfTheDustbound(Serial serial) : base(serial)
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
