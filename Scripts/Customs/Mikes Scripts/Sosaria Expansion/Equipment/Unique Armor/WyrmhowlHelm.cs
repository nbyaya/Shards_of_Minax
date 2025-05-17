using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WyrmhowlHelm : NorseHelm
{
    [Constructable]
    public WyrmhowlHelm()
    {
        Name = "Wyrmhowl Helm";
        Hue = Utility.Random(250, 2300); // Gives the helm a random hue from the specified range.
        BaseArmorRating = Utility.RandomMinMax(40, 80); // Randomly assigns a base armor rating between 40 and 80 for variety.

        // Bonus attributes fitting the Norse and dragon theme
        Attributes.BonusStr = 15; // Strength bonus to reflect the warrior’s might.
        Attributes.BonusDex = 10; // Dexterity to increase agility, in line with skilled combatants.
        Attributes.BonusInt = 5;  // Intelligence bonus for those who study the ways of the dragons and magic.
        Attributes.RegenHits = 3; // Slowly regenerate health in battle, reflecting the helm's ancient power.
        Attributes.DefendChance = 10; // A chance to defend against attacks, representing the helm’s mystical protection.

        // Skills related to combat, survival, and knowledge of beasts and dragons
        SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0); // Understanding of creatures' anatomy, fitting for a warrior of dragons.
        SkillBonuses.SetValues(1, SkillName.Tactics, 25.0);  // Bonus to Tactics, as the helm’s wearer is adept at planning battle strategies.
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 15.0); // Bonus to Animal Lore to signify understanding of mythical creatures, especially dragons.

        // Environmental protection bonuses fitting a piece tied to ancient and powerful beings
        ColdBonus = 10; // Protection from cold, which might reflect ancient ice dragon powers.
        FireBonus = 15; // Fire resistance, as dragons are often associated with fire.
        PhysicalBonus = 5; // Provides general physical defense against attacks.

        // Attaching the XmlLevelItem to ensure it integrates with the game system properly
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WyrmhowlHelm(Serial serial) : base(serial)
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
