using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VeilOfTheDrownedVoice : ChainCoif
{
    [Constructable]
    public VeilOfTheDrownedVoice()
    {
        Name = "Veil of the Drowned Voice";
        Hue = Utility.Random(2000, 2300); // Mysterious blue tones, like the deep sea.
        BaseArmorRating = Utility.RandomMinMax(25, 60); // Slightly higher armor value due to its rarity.

        // Attributes
        Attributes.BonusInt = 15;
        Attributes.BonusMana = 25;
        Attributes.SpellDamage = 10;
        Attributes.LowerManaCost = 10;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 15.0); // SpiritSpeak: Ties to the drowned spirits and whispers.
        SkillBonuses.SetValues(1, SkillName.Fishing, 20.0); // Fishing: Lore surrounding sailors and lost souls at sea.
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0); // Meditation: Calm, focusing the mind to hear the voices of the deep.

        // Elemental Bonuses
        ColdBonus = 10; // It is attuned to cold, like the deep, cold ocean.
        EnergyBonus = 15; // The whispers of drowned spirits emanate with energy.
        
        // Attach special properties using XmlLevelItem for rarity
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VeilOfTheDrownedVoice(Serial serial) : base(serial)
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
