using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WanderersMantleOfMist : Cloak
{
    [Constructable]
    public WanderersMantleOfMist()
    {
        Name = "Wanderer's Mantle of Mist";
        Hue = Utility.Random(2000, 2200); // A soft, ethereal hue fitting the cloak's nature

        // Set attributes and bonuses
        Attributes.BonusDex = 5; // For agility and swift movement
        Attributes.BonusInt = 10; // For wisdom and mystical connection
        Attributes.RegenMana = 3; // Subtle mana regeneration tied to the mist's ethereal nature
        Attributes.Luck = 25; // Increased luck for exploration and discovery

        // Resistances
        Resistances.Cold = 15; // Fits the misty environment, enhancing cold resistance
        Resistances.Poison = 5; // Slight resistance, useful in toxic mists or poisonous forests
        Resistances.Energy = 10; // As a cloak of mist, it also offers some energy protection

        // Skill Bonuses (Thematically tied to wandering, stealth, and mystical awareness)
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0); // For blending into the mist
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0); // To better navigate through the mist
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0); // Calm the mind and attune with the mystical mist
        SkillBonuses.SetValues(3, SkillName.Focus, 5.0); // Enhanced focus while traversing unknown paths

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WanderersMantleOfMist(Serial serial) : base(serial)
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
