using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DrakkonsRootedStance : DragonLegs
{
    [Constructable]
    public DrakkonsRootedStance()
    {
        Name = "Drakkon's Rooted Stance";
        Hue = 0x5B1; // A deep red, representing the dragon's bloodline.
        BaseArmorRating = 75; // A solid defensive value for a powerful item.

        Attributes.BonusStr = 15; // Drakkon’s strength passed down through the ages.
        Attributes.BonusHits = 30; // Rooted stance grants greater health and resistance.
        Attributes.DefendChance = 10; // Drakkon's ancient armor provides extra protection.

        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // Enhances battle tactics and positioning.
        SkillBonuses.SetValues(1, SkillName.Swords, 20.0); // Strengthens sword skills, as Drakkon was a warrior.

        ColdBonus = 5; // Minor protection against cold, reminiscent of Drakkon’s frigid northern origins.
        FireBonus = 10; // Enhanced protection against fire due to Drakkon’s fiery nature.

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DrakkonsRootedStance(Serial serial) : base(serial)
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
