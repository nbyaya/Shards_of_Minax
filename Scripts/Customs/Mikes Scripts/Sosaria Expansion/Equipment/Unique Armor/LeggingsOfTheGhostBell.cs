using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LeggingsOfTheGhostBell : ChainLegs
{
    [Constructable]
    public LeggingsOfTheGhostBell()
    {
        Name = "Leggings of the Ghost Bell";
        Hue = Utility.Random(2000, 2100); // A ghostly, ethereal hue, perhaps pale with undertones of blue or green
        BaseArmorRating = Utility.RandomMinMax(25, 55); // Moderate armor rating, suitable for stealth or elusive builds

        // Attributes tied to the item’s ethereal and mysterious nature
        Attributes.DefendChance = 10;  // Enhancing defense against attacks
        Attributes.BonusDex = 15;      // Boosting Dexterity, enhancing stealthy movement
        Attributes.BonusInt = 10;      // Adding a bit of magical intellect, as the armor draws from arcane energies
        Attributes.NightSight = 1;     // As a "ghost" item, granting the wearer the ability to see in the dark

        // Skill bonuses thematically connected to stealth and exploration
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);  // Boosting Stealth, perfect for hiding in plain sight
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0);  // Boosting Tracking, as the item might aid in following lost souls or hidden things
        SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 10.0); // The ethereal nature of the item connects with spirits, enhancing communication with them

        // Resistance bonuses that reflect the item’s ghostly origins
        ColdBonus = 10;
        PhysicalBonus = 5;

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LeggingsOfTheGhostBell(Serial serial) : base(serial)
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
